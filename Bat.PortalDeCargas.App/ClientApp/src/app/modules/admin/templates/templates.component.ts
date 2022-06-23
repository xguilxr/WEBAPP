import {Component, ViewEncapsulation} from '@angular/core';
import {Router} from '@angular/router';
import {PaginatedEntity} from '../../../model/paginated-entity';
import {Template} from '../../../model/template';
import {QueryFilter} from '../../../model/QueryFilter';
import {ColumnMode} from '@swimlane/ngx-datatable';
import {catchError, tap} from 'rxjs/operators';
import {TemplateService} from '../../../services/template.service';
import {DialogService} from '../../../services/dialog.service';
import {TranslocoService} from '@ngneat/transloco';
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
    selector: 'templates',
    templateUrl: './templates.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./template.component.scss']
})
export class TemplatesComponent {
    public templates: PaginatedEntity<Template> = {
        items: [],
        currentPage: 1,
        itemsPerPage: 1,
        totalOfPages: 1,
        totalOfItems: 1
    };
    public filter: QueryFilter = {
        name: '',
        page: 1
    };
    columnMode = ColumnMode;
    readonly messages = {
        emptyMessage: this.translocoService.translate('Template.emptyMessage'),
        totalMessage: this.translocoService.translate('Template.totalMessage')
    };

    constructor(private router: Router,
                private templateService: TemplateService,
                private dialogService: DialogService,
                private snackBar: MatSnackBar,
                private translocoService: TranslocoService) {
        this.getFilteredTemplates();
    }

    backToDashboard(): void {
        this.router.navigate(['dashboard']);
    }

    createTemplate(): void {
        this.router.navigate(['editTemplateForm']);
    }

    getFilteredTemplates(): void {
        this.templateService.getPaginated(this.filter)
        .pipe(catchError(() => this.dialogService.openErrorDialog(null, this.translocoService.translate('template.filterError'))))
        .subscribe(results => this.templates = {...results});
    }

    edit(templateId: number): void {
        this.router.navigate(['editTemplateForm', templateId]);
    }

    delete(value: number): void {
        this.templateService.delete(value)
        .pipe(tap(() => {
            const snackBarRef = this.snackBar.open(this.translocoService.translate('Template.deleteSuccess'), 'OK', {duration: 1500});
            snackBarRef
            .onAction()
            .pipe(tap(() => snackBarRef.dismiss()))
            .subscribe();
        }), tap(() => {
            this.getFilteredTemplates();
        }), catchError(() => this.dialogService.openErrorDialog(null, this.translocoService.translate('Template.deleteErro'))))
        .subscribe();
    }

    copyTemplate(value: number): void {
    }

    pageChanged($event: any): void {
    }
}
