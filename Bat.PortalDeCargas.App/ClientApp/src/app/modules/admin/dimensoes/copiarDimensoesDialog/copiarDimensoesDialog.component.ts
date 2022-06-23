import { Component, ViewEncapsulation,Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';

export interface copiarDimensaoData{ name:string}

@Component({
    selector     : 'copiarDimensoesDialog',
    templateUrl  : './copiarDimensoesDialog.component.html',
    encapsulation: ViewEncapsulation.None
})


export class CopiarDimensoesDialogComponent
{
    /**
     * Constructor
     */
     constructor(
        public dialogRef: MatDialogRef<CopiarDimensoesDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: copiarDimensaoData,
      ) {}
    
      onNoClick(): void {
        this.dialogRef.close();
      }
}
