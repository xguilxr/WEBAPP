import { tasks } from './../../../mock-api/apps/tasks/data';
import { DialogService } from './../../../services/dialog.service';
import { PaginatedDimensions } from 'app/model/paginated-dimension';
import { DimensionFilter } from 'app/model/dimension-filter';
import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { DimensionService } from 'app/services/dimension.service';
import {DatatableComponent} from '@swimlane/ngx-datatable';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { MatFormFieldControl } from '@angular/material/form-field';
import { catchError, filter, tap } from 'rxjs/operators';
import { TranslocoService } from '@ngneat/transloco';
import { MatSnackBar } from '@angular/material/snack-bar';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { CopiarDimensoesDialogComponent } from './copiarDimensoesDialog/copiarDimensoesDialog.component';
import fileDownload from 'js-file-download';




@Component({
    selector     : 'dimensoes',
    templateUrl: './dimensoes.component.html',
    encapsulation: ViewEncapsulation.None
})

export class DimensoesComponent
{
    public Dimensions: PaginatedDimensions = { items: [], currentPage: 1, itemsPerPage: 1, totalOfPages: 1, totalOfItems: 1 };
    public filter: DimensionFilter = { name: '', page: 1 };

    readonly messages = {
        emptyMessage:  this.translocoService.translate('Dimension.emptyMessage'),
        totalMessage:  this.translocoService.translate('Dimension.totalMessage')
      };

    ColumnMode = ColumnMode;



    constructor(private router: Router, private dimensionService: DimensionService ,
      private dialogService: DialogService,
      private translocoService: TranslocoService,
      private snackBar :MatSnackBar,
      private dialog: MatDialog) {
      this.getFilteredDimension();
    }

    createDimesion(): void {
        this.router.navigate(["criarDimensoes"]);
    }

    backToDashboard(): void {
        this.router.navigate(["dashboard"]);
    }

    copyDimension(dimensionId:number){

      var dimension = this.Dimensions.items.find( d => d.dimensionId == dimensionId);

      const dialogRef = this.dialog.open(CopiarDimensoesDialogComponent, {
        width: '350px',
        data: {name: 'Cópia '+ dimension.dimensionName},
      });

      dialogRef.afterClosed().subscribe(result => {
        var name = result;

        if (name != undefined)
          this.dimensionService.copy(dimensionId,name)
          .pipe(
            catchError((result) => this.dialogService.openErrorDialog(null, this.translocoService.translate('Dimension.copyErro')))
          )
          .subscribe(result =>
            {
              if (result.indSucesso) {
                const snackBarRef = this.snackBar.open(this.translocoService.translate('Dimension.copySuccess'), 'OK', { duration: 1500 });
                snackBarRef
                  .onAction()
                  .pipe(tap(() => snackBarRef.dismiss()))
                  .subscribe();

                  this.getFilteredDimension();
              }
              else
                this.dialogService.openErrorDialog(null, result.erros);
            })
      });
    }

    pageChanged({ offset }: { count: number; pageSize: number; limit: number; offset: number }): void {
        this.filter.page = offset + 1;
        this.getFilteredDimension()
      }

    edit(dimensionId:number){
      this.router.navigate(["criarDimensoes",dimensionId]);
    }

    deleteDimension(dimensionId:number){
      this.dimensionService
      .delete(dimensionId)
      .pipe(
        tap(() => {
          const snackBarRef = this.snackBar.open(this.translocoService.translate('Dimension.deleteSuccess'), 'OK', { duration: 1500 });
          snackBarRef
            .onAction()
            .pipe(tap(() => snackBarRef.dismiss()))
            .subscribe();
        }),
        tap(() => {
          const index = this.Dimensions.items.findIndex(d => d.dimensionId === dimensionId);
          this.Dimensions.items.splice(index, 1);
          this.Dimensions.items = [...this.Dimensions.items];
          this.getFilteredDimension();
        }),
        catchError(() => this.dialogService.openErrorDialog(null, this.translocoService.translate('Dimension.deleteErro')))
      )
      .subscribe();
    }

    delete(dimensionId:number){

      this.dialogService
      .openConfirmDialog(this.translocoService.translate('Dimension.deleteQuestion'))
      .pipe(
        filter(canContinue => !!canContinue),
        tap(() => this.deleteDimension(dimensionId))
      )
      .subscribe();
    }

    getFilteredDimension(){
        this.dimensionService.getPaginated(this.filter)
        .pipe(
          catchError(() => this.dialogService.openErrorDialog(null, this.translocoService.translate('Dimension.filterErro')))
        )
        .subscribe(results => this.Dimensions = {...results})
    }


    getExcelFile(){
      this.dimensionService.getExcelFile(this.filter.name)
      .subscribe( result =>
        fileDownload(result, "Dimension.xlsx")
        )

    }
}
