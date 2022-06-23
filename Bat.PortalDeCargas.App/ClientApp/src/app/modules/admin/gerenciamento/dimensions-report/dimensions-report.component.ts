import {Component, OnInit} from '@angular/core';
import {DimensionLog} from '../../../../model/dimension-log';
import {ColumnMode} from '@swimlane/ngx-datatable';
import {PaginatedEntity} from '../../../../model/paginated-entity';
import {TranslocoService} from '@ngneat/transloco';
import {DimensionLogFilter} from '../../../../model/dimension-log-filter';
import {DimensionService} from '../../../../services/dimension.service';
import {catchError} from 'rxjs/operators';
import {DialogService} from '../../../../services/dialog.service';

@Component({
    selector: 'dimensions-report',
    templateUrl: './dimensions-report.component.html',
    styleUrls: ['./dimensions-report.component.scss']
})
export class DimensionsReportComponent implements OnInit {
    public endDate: any;
    public startDate: any;
    public filter: DimensionLogFilter;
    public dimensionLog: PaginatedEntity<DimensionLog> = {
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
                private dimensionService: DimensionService,
                private dialogService: DialogService) {
    }

    ngOnInit(): void {
        this.filter = {
            dimensionName: '',
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
        this.dimensionService.getFilteredLog(this.filter)
        .pipe(catchError(() => this.dialogService.openErrorDialog(null, this.translocoService.translate('template.filterError'))))
        .subscribe((result) => {
            this.dimensionLog = {...result};
        });
    }

    pageChanged($event: any): void {
        this.filter.page = $event.offset + 1;
        this.getFilteredData();
    }
}
