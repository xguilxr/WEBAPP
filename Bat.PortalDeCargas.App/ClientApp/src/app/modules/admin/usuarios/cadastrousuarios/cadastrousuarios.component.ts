import { changePassword } from './../trocaSenhaDialog/trocaSenhaDialog.component';
import { UsersService } from 'app/services/users.service';
import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, filter, tap } from 'rxjs/operators';
import { TranslocoService } from '@ngneat/transloco';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AppUser  } from 'app/model/users';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DialogService } from 'app/services/dialog.service';



@Component({
    selector     : 'cadastrousuarios',
    templateUrl: './cadastrousuarios.component.html',
    encapsulation: ViewEncapsulation.None
})

export class CadastroUsuariosComponent implements OnInit
{
    userForm :FormGroup;
    userId: number = 0;
    AppUser: AppUser;

    ngOnInit(): void {

        let name = new FormControl('',Validators.required);
        let email = new FormControl('', Validators.required)
        let type = new FormControl('1',Validators.required);
        let password = new FormControl('',Validators.required);
        let confirmpassword = new FormControl('',Validators.required);

        this.userForm= new FormGroup({
            'name' : name,
            'email' : email,
            'type': type,
            'password' : password,
            'confirmpassword' : confirmpassword
           });


           this.activeRoute.paramMap.subscribe(params => {
            let userId = params.get("id");

            if (userId == null)
              return;

            this.userService.getUserById(userId).subscribe(
               result => {

                  this.AppUser = result;
                  this.userId = Number(userId);
                  this.AppUser.userId = this.userId

                  name.patchValue(result.userName);
                  email.patchValue(result.userEmail);
                  type.patchValue(result.userType.toString());

              });

          });




    }

    constructor (private router: Router ,
        private userService: UsersService,
        private dialogService: DialogService,
        private translocoService: TranslocoService,
        private snackBar :MatSnackBar,
        private activeRoute :ActivatedRoute,){

    }

    backToTemplates(): void {
        this.router.navigate(["usuarios"]);
    }



    onSubmit(){
        var FormValues =  {...this.userForm.value};
        var format ="";

        var User:AppUser =
        {
            userId : this.userId,
            userName: FormValues.name,
            userEmail:FormValues.email,
            userType : Number(FormValues.type),
            password: FormValues.password,
            confirmPassword: FormValues.confirmpassword
        };



        if (User.userName.trim() === ""){
            this.dialogService.openErrorDialog(null, this.translocoService.translate('User.userNameFill'))
            return;
        }

        if (User.userEmail.trim() === ""){
            this.dialogService.openErrorDialog(null, this.translocoService.translate('User.userEmailFill'))
            return;
        }

        if (FormValues.confirmpassword != FormValues.password){
            this.dialogService.openErrorDialog(null, this.translocoService.translate('User.userPasswordInvalid'))
            return;
        }

        var emailReg =  /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

        if(!emailReg.test(User.userEmail.trim() ) ){
            this.dialogService.openErrorDialog(null, this.translocoService.translate('emailInvalid'))
            return;
        }


        this.userService.save(User)
        .pipe(
          catchError((result) => this.dialogService.openErrorDialog(null, this.translocoService.translate('User.saveErro')))
        )
        .subscribe(result =>
            {
                if (result.indSucesso) {


                  this.userId = result.model.userId

                  const snackBarRef = this.snackBar.open(this.translocoService.translate('User.saveSuccess'), 'OK', { duration: 1500 });
                  snackBarRef
                    .onAction()
                    .pipe(tap(() => snackBarRef.dismiss()))
                    .subscribe();
                }
                else
                  this.dialogService.openErrorDialog(null, result.erros);
            }
          );

        }




}
