import { Component, ViewEncapsulation,Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { TranslocoService } from '@ngneat/transloco';
import { String } from 'lodash';

export interface changePassword{ password:string; confirmPassword:string}

@Component({
    selector     : 'trocaSenhaDialog',
    templateUrl  : './trocaSenhaDialog.component.html',
    encapsulation: ViewEncapsulation.None
})


export class TrocaSenhaDialogComponent
{
    /**
     * Constructor
     */

     msgPassword:string;
     msgConfirmPassword:string;

     constructor(
        public dialogRef: MatDialogRef<TrocaSenhaDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: changePassword,
        private translocoService: TranslocoService,
      ) {

        }
    
      onNoClick(): void {
        this.dialogRef.close();
      }

      onClick():void{
        this.msgPassword  = "";
        this.msgConfirmPassword = "";        
        
        if (this.data.password.trim() == "" ){
            this.msgPassword = this.translocoService.translate('User.userPasswordFill');
            return;
          }
          if (this.data.confirmPassword.trim() == "" ){
            this.msgConfirmPassword = this.translocoService.translate('User.userConfirmPasswordFill');
            return;
          }
          if (this.data.confirmPassword != this.data.password ){
            this.msgConfirmPassword = this.translocoService.translate('User.userPasswordInvalid');
            return;
          }
          this.dialogRef.close({data:this.data});  
      }
}
