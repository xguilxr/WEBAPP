import {DimensionService} from './../../../../services/dimension.service';
import {Component, ElementRef, OnInit, ViewChild, ViewEncapsulation} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import {AbstractControl, FormControl, FormGroup, ValidatorFn, Validators} from '@angular/forms';
import {MatAutocompleteSelectedEvent} from '@angular/material/autocomplete';
import {MatChipInputEvent} from '@angular/material/chips';
import {Observable} from 'rxjs';
import {catchError, filter, map, startWith, tap} from 'rxjs/operators';
import {DimensionFormDTO} from 'app/model/DimensionFormDTO';
import {TranslocoService} from '@ngneat/transloco';
import {MatSnackBar} from '@angular/material/snack-bar';
import {MatDialog} from '@angular/material/dialog';
import {DialogService} from 'app/services/dialog.service';
import fileDownload from 'js-file-download';
import {DominioDialogComponent} from '../DominioDialog/dominiodialog.component';
import {Dimension} from 'app/model/dimension';

@Component({
    selector: 'criarDimensoes',
    templateUrl: './criarDimensoes.component.html',
    encapsulation: ViewEncapsulation.None
})
export class CriarDimensoesComponent implements OnInit {
    separatorKeysCodes: number[] = [ENTER, COMMA];
    patternCtrl = new FormControl();
    filteredPatterns: Observable<string[]>;
    patterns: string[] = [];
    allPatterns: string[] = ['Ano', 'Mês', 'Dia', '-', '/'];
    patternsConversion: any[] = [{
        de: 'Ano',
        para: 'yyyy'
    }, {
        de: 'Mês',
        para: 'MM'
    }, {
        de: 'Dia',
        para: 'dd'
    }, {
        de: '-',
        para: '-'
    }, {
        de: '/',
        para: '/'
    }];
    dimensionForm: FormGroup;
    dimensionId: number;
    amountItemsUpload: number;
    msgUpload: string = 'Carregue o Arquivo';
    dimension: Dimension;
    @ViewChild('patternInput') patternInput: ElementRef<HTMLInputElement>;
    @ViewChild('fileInputDomain') fileImputDomain: ElementRef<HTMLInputElement>;
    @ViewChild('fileInputTest') fileInputTest: ElementRef<HTMLInputElement>;

    constructor(private router: Router,
                private dimensionService: DimensionService,
                private dialogService: DialogService,
                private translocoService: TranslocoService,
                private snackBar: MatSnackBar,
                private activeRoute: ActivatedRoute,
                private dlgDoman: MatDialog) {
        this.filteredPatterns = this.patternCtrl.valueChanges.pipe(startWith(null), map((pattern: string | null) => (pattern ? this._filter(pattern) : this.allPatterns.slice())),);
    }

    ngOnInit(): void {
        const type = new FormControl('1', Validators.required);
        const format = new FormControl('');
        const size = new FormControl('');
        const initialvalue = new FormControl('');
        const finalvalue = new FormControl('');
        const name = new FormControl('', Validators.required);
        const description = new FormControl('', Validators.required);
        format.validator = this.ValidateFormat(format, type);
        this.dimensionForm = new FormGroup({
            'name': name,
            'description': description,
            'type': type,
            'size': size,
            'initialvalue': initialvalue,
            'finalvalue': finalvalue,
            'format': format
        });
        type.valueChanges.subscribe((selectedValue) => {
            if (selectedValue == '1') {
                size.patchValue('');
                format.patchValue('');
                this.patterns = [];
            }
            if (selectedValue == '2') {
                size.patchValue('');
                format.patchValue('');
                initialvalue.patchValue('');
                finalvalue.patchValue('');
                this.patterns = [];
            }
            if (selectedValue == '3') {
                size.patchValue('');
                initialvalue.patchValue('');
                finalvalue.patchValue('');
            }
        });
        this.activeRoute.paramMap.subscribe((params) => {
            const dimensionId = params.get('id');
            if (dimensionId == null) {
                return;
            }
            this.dimensionService.getDimensionById(dimensionId).subscribe((result) => {
                this.dimension = result;
                this.amountItemsUpload = this.dimension.domains.length;
                if (result.dimensionFormat != null && result.dimensionFormat != '') {
                    const showFormat = result.dimensionFormat.replace('yyyy', 'Ano;')
                        .replace('MM', 'Mês;').replace('dd', 'Dia;');
                    const formatarray = showFormat.split(';');
                    formatarray.forEach((element) => {
                        if (element.substring(0, 1) == '/') {
                            this.patterns.push('/');
                        }
                        if (element.substring(0, 1) == '-') {
                            this.patterns.push('-');
                        }
                        element = element.replace('/', '').replace('-', '');
                        if (element != '') {
                            this.patterns.push(element);
                        }
                    });
                }
                this.dimensionId = Number(dimensionId);
                size.patchValue(result.dimensionSize);
                format.patchValue(result.dimensionFormat);
                type.patchValue(result.dimensionType.toString());
                name.patchValue(result.dimensionName);
                description.patchValue(result.dimensionDescription);
                initialvalue.patchValue(result.dimensionStartNumber);
                finalvalue.patchValue(result.dimensionEndNumber);
            });
        });
    }

    ValidateFormat(Format: FormControl,
                   type: FormControl): ValidatorFn {
        return (control: AbstractControl): { [key: string]: any } | null => {
            if (type.value == 3 && Format.value == '') {
                return {'FormatInvalid': {value: Format.value}};
            } else {
                return null;
            }
        };
    }

    backToTemplates(): void {
        this.router.navigate(['dimensoes']);
    }

    add(event: MatChipInputEvent): void {
        const value = (event.value || '').trim();
        if (value) {
            this.patterns.push(value);
        }
        // Clear the input value
        event.chipInput!.clear();
        this.patternCtrl.setValue(null);
    }

    remove(pattern: string): void {
        const index = this.patterns.indexOf(pattern);
        if (index >= 0) {
            this.patterns.splice(index, 1);
        }
    }

    selected(event: MatAutocompleteSelectedEvent): void {
        this.patterns.push(event.option.viewValue);
        this.patternInput.nativeElement.value = '';
        this.patternCtrl.setValue(null);
    }

    onSubmit() {
        const FormValues = {...this.dimensionForm.value};
        let format = '';
        this.patterns.forEach((element) => {
            const addFormat = this.patternsConversion.find(p => p.De == element);
            format += addFormat.Para;
        });
        const dimensionForm: DimensionFormDTO = {
            id: this.dimensionId,
            name: FormValues.name,
            description: FormValues.description,
            type: Number(FormValues.type),
            size: FormValues.size == '' ? null : Number(FormValues.size),
            format: format,
            endNumber: FormValues.finalvalue == '' ? null : Number(FormValues.finalvalue),
            startNumber: FormValues.initialvalue == '' ? null : Number(FormValues.initialvalue),
        };
        this.dimensionService.save(dimensionForm)
            .pipe(catchError(result => this.dialogService.openErrorDialog(null, this.translocoService.translate('Dimension.saveErro'))))
            .subscribe((result) => {
                if (result.indSucesso) {
                    this.dimension = result.model;
                    this.dimensionId = result.model.dimensionId;
                    const snackBarRef = this.snackBar.open(this.translocoService.translate('Dimension.saveSuccess'), 'OK', {duration: 1500});
                    snackBarRef
                        .onAction()
                        .pipe(tap(() => snackBarRef.dismiss()))
                        .subscribe();
                } else {
                    this.dialogService.openErrorDialog(null, result.erros);
                }
            });
    }

    uploadDomainFile(file: File) {
        this.fileImputDomain.nativeElement.value = '';
        this.dimensionService.uploadDimensionDomain(file, this.dimensionId)
            .pipe(catchError(result => this.dialogService.openErrorDialog(null, this.translocoService.translate('Dimension.uploadErro'))))
            .subscribe((result) => {
                if (result.indSucesso) {
                    this.msgUpload = this.translocoService.translate('Dimension.uploadSuccess');
                    this.dimension = result.model;
                    this.amountItemsUpload = this.dimension.domains.length;
                    const snackBarRef = this.snackBar.open(this.translocoService.translate('Dimension.uploadSuccess'), 'OK', {duration: 1500});
                    snackBarRef
                        .onAction()
                        .pipe(tap(() => snackBarRef.dismiss()))
                        .subscribe();
                } else {
                    this.dialogService.openErrorDialog(null, result.erros[0]);
                }
            });
    }

    uploadTestFile(file: File) {
        this.fileInputTest.nativeElement.value = '';
        this.dimensionService.uploadDimensionTest(file, this.dimensionId)
            .pipe(catchError(result => this.dialogService.openErrorDialog(null, this.translocoService.translate('Dimension.uploadErro'))))
            .subscribe((result) => {
                fileDownload(result, 'criticas.xlsx');
            });
    }

    showDimensionDomain() {
        const dialogRef = this.dlgDoman.open(DominioDialogComponent, {
            width: '350px',
            data: {domains: this.dimension.domains},
        });
    }

    delete() {
        this.dialogService
            .openConfirmDialog(this.translocoService.translate('Dimension.domainDeleteQuestion'))
            .pipe(filter(canContinue => !!canContinue), tap(() => this.deleteDomain()))
            .subscribe();
    }

    deleteDomain() {
        this.dimensionService
            .deleteDomain(this.dimensionId)
            .pipe(tap(() => {
                const snackBarRef = this.snackBar.open(this.translocoService.translate('Dimension.domainDeleteSuccess'), 'OK', {duration: 1500});
                snackBarRef
                    .onAction()
                    .pipe(tap(() => snackBarRef.dismiss()))
                    .subscribe();
            }), tap(() => {
                this.amountItemsUpload = 0;
                this.dimension.domains = [];
            }), catchError(() => this.dialogService.openErrorDialog(null, this.translocoService.translate('Dimension.domainDeleteErro'))))
            .subscribe();
    }

    private _filter(value: string): string[] {
        const filterValue = value.toLowerCase();
        return this.allPatterns.filter(pattern => pattern.toLowerCase().includes(filterValue));
    }
}
