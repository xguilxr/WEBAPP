import { DialogService } from './../../../services/dialog.service';
import { UsersService } from 'app/services/users.service';
import { UsersFilter } from './../../../model/users.filter';
import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { ColumnMode } from '@swimlane/ngx-datatable';
import { catchError, filter, tap } from 'rxjs/operators';
import { TranslocoService } from '@ngneat/transloco';
import { MatSnackBar } from '@angular/material/snack-bar';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { AppUser, ChangePassword } from 'app/model/users';
import { Paginated } from 'app/model/paginated';
import { UserType } from 'app/model/user.type';
import { TrocaSenhaDialogComponent } from './trocaSenhaDialog/trocaSenhaDialog.component';


@Component({
    selector     : 'usuarios',
    templateUrl: './usuarios.component.html',
    encapsulation: ViewEncapsulation.None
})

export class UsuariosComponent
{
    public Users: Paginated<AppUser> = { items: [], currentPage: 1, itemsPerPage: 1, totalOfPages: 1, totalOfItems: 1 };
    public filter: UsersFilter = { name: '', page: 1 };


    readonly messages = {
        emptyMessage:  this.translocoService.translate('User.emptyMessage'),
        totalMessage:  this.translocoService.translate('User.totalMessage')
      };

    ColumnMode = ColumnMode;

    constructor(private router: Router, private usersService: UsersService ,
      private translocoService: TranslocoService,
      private snackBar :MatSnackBar,
      private dialog: MatDialog,
      private dialogService: DialogService) {
      this.getPaginatedUser();
    }

    getType(userType : UserType){

      if (userType == UserType.Administrador)
        return this.translocoService.translate('administrator');
      else
        return this.translocoService.translate('regular');

    }

    createUser(): void {
        this.router.navigate(["cadastrousuarios"]);
    }

    backToDashboard(): void {
        this.router.navigate(["dashboard"]);
    }



    pageChanged({ offset }: { count: number; pageSize: number; limit: number; offset: number }): void {
        this.filter.page = offset + 1;
        this.getPaginatedUser()
      }

    edit(userId:number){
       this.router.navigate(["cadastrousuarios",userId]);
    }

    deleteUser(userId:number){
      this.usersService
      .delete(userId)
      .pipe(
        tap(() => {
          const snackBarRef = this.snackBar.open(this.translocoService.translate('User.deleteSuccess'), 'OK', { duration: 1500 });
          snackBarRef
            .onAction()
            .pipe(tap(() => snackBarRef.dismiss()))
            .subscribe();
        }),
        tap(() => {
          const index = this.Users.items.findIndex(u => u.userId === userId);
          this.Users.items.splice(index, 1);
          this.Users.items = [...this.Users.items];
          this.getPaginatedUser();
        }),
        catchError(() => this.dialogService.openErrorDialog(null, this.translocoService.translate('User.deleteErro')))
      )
      .subscribe();
    }

    delete(userId:number){

      this.dialogService
      .openConfirmDialog(this.translocoService.translate('User.deleteQuestion'))
      .pipe(
        filter(canContinue => !!canContinue),
        tap(() => this.deleteUser(userId))
      )
      .subscribe();
    }

    getPaginatedUser(){
        this.usersService.getPaginated(this.filter)
        .pipe(
          catchError(() => this.dialogService.openErrorDialog(null, this.translocoService.translate('User.filterErro')))
        )
        .subscribe(result => this.Users =  result as Paginated<AppUser>)
    }


    changePassword( userId:number){

      const dialogRef = this.dialog.open(TrocaSenhaDialogComponent, {
        width: '450px',
        disableClose: true,
        data: {password:'', confirmPassword:'' },
      });

      dialogRef.afterClosed().subscribe(result => {

       if (result != undefined)
       {
          var Password:ChangePassword =
          {
              userId : userId,
              password: result.data.password,
              confirmPassword: result.data.confirmPassword
          };

          this.usersService.changePassword(Password)
          .pipe(
            catchError((result) => this.dialogService.openErrorDialog(null, this.translocoService.translate('User.changePasswordErro')))
          )
          .subscribe(result =>
            {

              if (result.indSucesso) {
                const snackBarRef = this.snackBar.open(this.translocoService.translate('User.changePasswordSuccess'), 'OK', { duration: 1500 });
                snackBarRef
                  .onAction()
                  .pipe(tap(() => snackBarRef.dismiss()))
                  .subscribe();


              }
              else
                this.dialogService.openErrorDialog(null, result.erros);
            }
          )
       }
      });
    }

}
