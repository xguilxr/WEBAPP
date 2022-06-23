import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import {Router} from '@angular/router';
import {TranslocoService} from '@ngneat/transloco';
import {QueryFilter} from '../../../model/QueryFilter';
import {catchError} from 'rxjs/operators';
import {TemplateService} from '../../../services/template.service';
import {DialogService} from '../../../services/dialog.service';
import {PaginatedEntity} from '../../../model/paginated-entity';
import {Template} from '../../../model/template';
import {ColumnMode} from '@swimlane/ngx-datatable';

@Component({
    selector: 'uploads',
    templateUrl: './uploads.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['uploads.component.scss']
})
export class UploadsComponent implements OnInit {
    public filter: QueryFilter = {
        name: '',
        page: 1,
        pageCount: 5
    };
    public templates: PaginatedEntity<Template> = {
        items: [],
        currentPage: 1,
        itemsPerPage: 1,
        totalOfPages: 1,
        totalOfItems: 1
    };
    columnMode = ColumnMode;
    readonly messages = {
        emptyMessage: this.translocoService.translate('TemplateUpload.emptyMessage'),
        totalMessage: this.translocoService.translate('TemplateUpload.totalMessage')
    };

    constructor(private router: Router,
                private translocoService: TranslocoService,
                private templateService: TemplateService,
                private dialogService: DialogService) {
    }

    ngOnInit(): void {
        this.getFilteredTemplates();
    }

    backToDashboard(): void {
        this.router.navigate(['dashboard']);
    }

    goToUpload(templateId: number): void {
        this.router.navigate(['upload', templateId]);
    }

    getFilteredTemplates(): void {
        this.templateService.getPaginated(this.filter)
            .pipe(catchError(() => this.dialogService.openErrorDialog(null, this.translocoService.translate('TemplateUpload.filterError'))))
            .subscribe(results => this.templates = {...results});
    }

    pageChanged($event: any): void {
    }
}
