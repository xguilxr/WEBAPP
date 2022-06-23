import {Component, OnInit} from '@angular/core';
import {ColumnMode} from '@swimlane/ngx-datatable';
import {PaginatedEntity} from '../../../../model/paginated-entity';
import {TranslocoService} from '@ngneat/transloco';
import {catchError} from 'rxjs/operators';
import {DialogService} from '../../../../services/dialog.service';
import {TemplateService} from '../../../../services/template.service';
import {TemplateLog} from '../../../../model/template-log';
import {TemplateLogFilter} from '../../../../model/template-log-filter';

@Component({
    selector: 'template-report',
    templateUrl: './template-report.component.html',
    styleUrls: ['./template-report.component.scss']
})
export class TemplateReportComponent implements OnInit {
    public endDate: any;
    public startDate: any;
    public filter: TemplateLogFilter;
    public templateLog: PaginatedEntity<TemplateLog> = {
        items: [],
        currentPage: 1,
        itemsPerPage: 1,
        totalOfPages: 1,
        totalOfItems: 1
    };
    columnMode = ColumnMode;
    readonly messages = {
        emptyMessage: this.translocoService.translate('Template.emptyMessage'),
        totalMessage: this.translocoService.translate('Template.totalMessage')
    };

    constructor(private translocoService: TranslocoService,
                private templateService: TemplateService,
                private dialogService: DialogService) {
    }

    ngOnInit(): void {
        this.filter = {
            templateName: '',
            endDate: undefined,
            page: 1,
            pageSize: 20,
            startDate: undefined,
            userName: ''
        };
    }

    getFilteredData(): void {
        this.filter.endDate = this.endDate ? this.endDate.toISOString() : null;
        this.filter.startDate = this.startDate ? this.startDate.toISOString() : null;
        this.templateService.getFilteredLog(this.filter)
        .pipe(catchError(() => this.dialogService.openErrorDialog(null, this.translocoService.translate('template.filterError'))))
        .subscribe((result) => {
            this.templateLog = {...result};
        });
    }

    pageChanged($event: any): void {
        this.filter.page = $event.offset + 1;
        this.getFilteredData();
    }
}
