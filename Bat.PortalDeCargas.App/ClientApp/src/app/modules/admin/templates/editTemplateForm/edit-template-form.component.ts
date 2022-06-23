import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {catchError, tap} from 'rxjs/operators';
import {TemplateService} from '../../../../services/template.service';
import {Template} from '../../../../model/template';
import {TemplateFormDTO} from '../../../../model/templateFormDTO';
import {DialogService} from '../../../../services/dialog.service';
import {TranslocoService} from '@ngneat/transloco';
import {MatSnackBar} from '@angular/material/snack-bar';
import {ColumnMode} from '@swimlane/ngx-datatable';
import {Dimension} from '../../../../model/dimension';
import {DimensionService} from '../../../../services/dimension.service';
import {TemplateDimension} from '../../../../model/templateDimension';

@Component({
    selector: 'editTemplateForm',
    templateUrl: './edit-template-form.component.html',
    encapsulation: ViewEncapsulation.None
})
export class EditTemplateFormComponent implements OnInit {
    public templateForm: FormGroup;
    public readonly messages = {
        emptyMessage: this.translocoService.translate('Template.emptyMessage'),
        totalMessage: this.translocoService.translate('Template.totalMessage')
    };
    public template: Template;
    columnMode = ColumnMode;
    public newTemplateDimension: TemplateDimension;
    public dimensionList: Dimension[];
    public isValidNewDimension: boolean;
    private nextDimensionId: number;

    constructor(private router: Router,
                private activeRoute: ActivatedRoute,
                private templateService: TemplateService,
                private dimensionService: DimensionService,
                private dialogService: DialogService,
                private translocoService: TranslocoService,
                private snackBar: MatSnackBar) {
    }

    backToTemplates(): void {
        this.router.navigate(['templates']);
    }

    ngOnInit(): void {
        this.isValidNewDimension = true;
        this.newDimension();
        this.dimensionService.getAllDimensions().subscribe(value => this.dimensionList = value);
        const name = new FormControl('', Validators.required);
        const fileFormat = new FormControl('1', Validators.required);
        const description = new FormControl('', Validators.required);
        const periodicity = new FormControl('1', Validators.required);
        const endUploadWindow = new FormControl('', Validators.required);
        const blobAddress = new FormControl('', Validators.required);
        const notificationEmail = new FormControl('', Validators.required);
        const notificationText = new FormControl('', Validators.required);
        const validation = new FormControl('');
        const updateFeatures = new FormControl('');
        this.templateForm = new FormGroup({
            'name': name,
            'fileFormat': fileFormat,
            'description': description,
            'periodicity': periodicity,
            'endUploadWindow': endUploadWindow,
            'blobAddress': blobAddress,
            'notificationEmail': notificationEmail,
            'notificationText': notificationText,
            'validation': validation,
            'updateFeatures': updateFeatures
        });
        this.activeRoute.paramMap.subscribe((params) => {
            if (params.get('id') == null) {
                this.nextDimensionId = 1;
                this.template = {
                    createDate: new Date(),
                    createUserId: 0,
                    createUserName: '',
                    dimensions: [],
                    templateBlobUrl: '',
                    templateDescription: '',
                    templateEndUploadWindow: 0,
                    templateFileFormat: {name: '', value: 0},
                    templateId: 0,
                    templateName: '',
                    templateNotificationEmail: '',
                    templateNotificationText: '',
                    templatePeriodicity: 0,
                    templateUpdateFeatures: '',
                    templateValidation: '',
                    updateDate: null,
                    updateUserId: null,
                    updateUserName: null,
                    deleted: []
                };
            } else {
                this.templateService.getTemplateById(Number(params.get('id'))).subscribe((result) => {
                    this.template = result;
                    name.patchValue(result.templateName);
                    fileFormat.patchValue(result.templateFileFormat.value);
                    description.patchValue(result.templateDescription);
                    periodicity.patchValue(String(result.templatePeriodicity));
                    endUploadWindow.patchValue(Number(result.templateEndUploadWindow));
                    blobAddress.patchValue(result.templateBlobUrl);
                    notificationEmail.patchValue(result.templateNotificationEmail);
                    notificationText.patchValue(result.templateNotificationText);
                    validation.patchValue(result.templateValidation);
                    updateFeatures.patchValue(result.templateUpdateFeatures);
                    this.nextDimensionId = Math.max(...this.template.dimensions.map(d => d.templateId));
                });
            }
        });
    }

    onSubmit(): void {
        const formValues = {...this.templateForm.value};
        const templateId = this.template === undefined ? 0 : this.template.templateId;
        const templateForm: TemplateFormDTO = {
            dimensions: this.template.dimensions.map(d => ({
                date: new Date(),
                dimensionId: d.dimensionId,
                dimensionName: d.dimensionName,
                id: (d.isNew ? 0 : d.templateDimensionId),
                name: d.templateDimensionName,
                order: d.templateDimensionOrder,
                templateId: d.templateId,
                userId: d.userId,
                userName: d.userName,
                isUpdated: d.isUpdated
            })),
            endUploadWindow: Number(formValues.endUploadWindow),
            notificationEmail: formValues.notificationEmail,
            notificationText: formValues.notificationText,
            validation: formValues.validation,
            id: templateId,
            description: formValues.description,
            blobUrl: formValues.blobAddress,
            updateFeatures: formValues.updateFeatures,
            fileFormat: Number(formValues.fileFormat),
            name: formValues.name,
            periodicity: Number(formValues.periodicity),
            deleted: this.template.deleted
        };
        this.templateService.save(templateForm)
            .pipe(catchError(result => this.dialogService.openErrorDialog(null, this.translocoService.translate('Template.Cadastro.saveErro'))))
            .subscribe((result) => {
                if (result.indSucesso) {
                    this.template = result.model;
                    const snackBarRef = this.snackBar.open(this.translocoService.translate('Template.Cadastro.saveSuccess'), 'OK', {duration: 1500});
                    this.router.navigate(['templates']);
                    snackBarRef.onAction().pipe(tap(() => {
                        snackBarRef.dismiss();
                    })).subscribe();
                } else {
                    this.dialogService.openErrorDialog(null, result.erros);
                }
            });
    }

    edit(value: number): void {
        this.newTemplateDimension = this.template.dimensions.find(d => d.templateDimensionId === value);
    }

    delete(value: number): void {
        const idx = this.template.dimensions.findIndex(d => d.templateDimensionId === value);
        const dimensao = this.template.dimensions.splice(idx, 1)[0];
        const dimensoes = this.template.dimensions.filter(d => d.templateDimensionOrder > dimensao.templateDimensionOrder);
        dimensoes.forEach((d,
                           i,
                           a) => {
            d.isUpdated = !d.isNew;
            d.templateDimensionOrder--;
            this.template.dimensions = [...this.template.dimensions];
        });
        if (!dimensao.isNew) {
            this.template.deleted.push(dimensao.templateDimensionId);
        }
    }

    public addDimension(): void {
        if (this.newTemplateDimension.dimensionId === 0 || this.newTemplateDimension.templateDimensionName === '') {
            this.isValidNewDimension = false;
        } else {
            const selectedDimension = this.dimensionList.find(d => d.dimensionId === this.newTemplateDimension.dimensionId);
            if (this.newTemplateDimension.isNew) {
                this.isValidNewDimension = true;
                const newDimension: TemplateDimension = {
                    dimensionId: this.newTemplateDimension.dimensionId,
                    dimensionName: selectedDimension.dimensionName,
                    templateDimensionDate: new Date(),
                    templateDimensionId: this.nextDimensionId++,
                    templateDimensionName: this.newTemplateDimension.templateDimensionName,
                    templateDimensionOrder: this.template.dimensions.length + 1,
                    templateId: this.template.templateId,
                    userId: 1,
                    userName: '',
                    isNew: true,
                    isUpdated: false
                };
                this.template.dimensions.push(newDimension);
                this.template.dimensions = [...this.template.dimensions];
            } else {
                const dimensao = this.template.dimensions.find(d => d.templateDimensionId === this.newTemplateDimension.templateDimensionId);
                dimensao.templateDimensionDate = new Date();
                dimensao.dimensionName = selectedDimension.dimensionName;
                dimensao.isUpdated = true;
            }
            this.newDimension();
        }
    }

    up(value: number): void {
        const dimensao = this.template.dimensions.find(d => d.templateDimensionId === value);
        if (dimensao.templateDimensionOrder > 1) {
            const outraDimensao = this.template.dimensions.find(d => d.templateDimensionOrder === dimensao.templateDimensionOrder - 1);
            outraDimensao.templateDimensionOrder++;
            outraDimensao.isUpdated = true;
            dimensao.templateDimensionOrder--;
            dimensao.isUpdated = true;
            this.template.dimensions = [...this.template.dimensions];
        }
    }

    down(value: number): void {
        const dimensao = this.template.dimensions.find(d => d.templateDimensionId === value);
        if (dimensao.templateDimensionOrder < this.template.dimensions.length) {
            const outraDimensao = this.template.dimensions.find(d => d.templateDimensionOrder === dimensao.templateDimensionOrder + 1);
            outraDimensao.templateDimensionOrder--;
            outraDimensao.isUpdated = true;
            dimensao.templateDimensionOrder++;
            dimensao.isUpdated = true;
            this.template.dimensions = [...this.template.dimensions];
        }
    }

    private newDimension(): void {
        this.newTemplateDimension = {
            dimensionId: 0,
            dimensionName: '',
            isNew: true,
            templateDimensionDate: undefined,
            templateDimensionId: this.nextDimensionId++,
            templateDimensionName: '',
            templateDimensionOrder: 0,
            templateId: 0,
            userId: 0,
            userName: '',
            isUpdated: false
        };
    }
}
