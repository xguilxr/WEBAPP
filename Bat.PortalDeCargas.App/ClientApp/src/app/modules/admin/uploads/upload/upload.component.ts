import {Component, ElementRef, OnInit, ViewChild, ViewEncapsulation} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {Template} from '../../../../model/template';
import {TemplateService} from '../../../../services/template.service';
import {TranslocoService} from '@ngneat/transloco';
import fileDownload from 'js-file-download';
import {ColumnMode} from '@swimlane/ngx-datatable';
import {UploadResult} from '../../../../model/uploadResult';

@Component({
    selector: 'upload',
    templateUrl: './upload.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['upload.component.scss']
})
export class UploadComponent implements OnInit {
    @ViewChild('fileInput') fileInput: ElementRef;
    public template: Template;
    columnMode = ColumnMode;
    readonly messages = {
        emptyMessage: this.translocoService.translate('TemplateUpload.emptyMessage'),
        totalMessage: this.translocoService.translate('TemplateUpload.totalMessage')
    };
    public uploadResult: UploadResult;

    constructor(private router: Router,
                private activeRoute: ActivatedRoute,
                private templateService: TemplateService,
                private translocoService: TranslocoService) {
    }

    backToDashboard(): void {
        this.router.navigate(['uploads']);
    }

    ngOnInit(): void {
        this.uploadResult = null;
        this.activeRoute.paramMap.subscribe((params) => {
            this.templateService.getTemplateById(Number(params.get('id'))).subscribe((result) => {
                this.template = result;
            });
        });
    }

    downloadTemplate(templateId: number): void {
        this.templateService.getFileTemplate(templateId).subscribe(result => fileDownload(result, 'template.xlsx'));
    }

    uploadFile($event: Event): void {
        const file = ($event.target as HTMLInputElement).files[0];
        this.templateService.uploadFile(file, this.template.templateId).subscribe((result) => {
            fileDownload(result, 'template.xlsx');
        }, null, () => {
            this.templateService.getUploadResult(this.template.templateId).subscribe(result => this.uploadResult = result);
        });
    }
}
