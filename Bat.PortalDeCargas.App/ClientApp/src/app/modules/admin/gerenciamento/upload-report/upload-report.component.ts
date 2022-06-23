import {Component, OnInit, ViewChild} from '@angular/core';
import {PaginatedEntity} from '../../../../model/paginated-entity';
import {ColumnMode} from '@swimlane/ngx-datatable';
import {UploadLog} from '../../../../model/upload-log';
import {TranslocoService} from '@ngneat/transloco';
import {UploadLogFilter} from '../../../../model/upload-log-filter';
import {catchError} from 'rxjs/operators';
import {TemplateService} from '../../../../services/template.service';
import {DialogService} from '../../../../services/dialog.service';

@Component({
    selector: 'upload-report',
    templateUrl: './upload-report.component.html',
    styleUrls: ['./upload-report.component.scss']
})
export class UploadReportComponent implements OnInit {
    @ViewChild('myTable') table: any;
    public endDate: any;
    public startDate: any;
    public filter: UploadLogFilter;
    public log: PaginatedEntity<UploadLog> = {
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

    constructor(private templateService: TemplateService,
                private translocoService: TranslocoService,
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
        this.templateService.getFilteredUpdateLog(this.filter)
        .pipe(catchError(() => this.dialogService.openErrorDialog(null, this.translocoService.translate('template.filterError'))))
        .subscribe((result) => {
            this.log = {...result};
        });
    }

    pageChanged($event: any): void {
        this.filter.page = $event.offset + 1;
        this.getFilteredData();
    }

    onDetailToggle($event: any) {
        console.log("onDetailToggle", $event);
    }

    toggleExpandRow(row: UploadLog): void {
        if (row.details.totalOfItems === 0) {
            this.templateService.getUploadDetail({
                uploadLogId: row.uploadLogId,
                page: 1,
                pageSize: 20
            }).subscribe(result => row.details = result);
        }
        this.table.rowDetail.toggleExpandRow(row);
    }
}
