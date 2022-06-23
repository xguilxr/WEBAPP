import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogConfig, DialogType } from '../../model/dialog';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.scss']
})
export class DialogComponent {
  config: DialogConfig;

  constructor(public dialogRef: MatDialogRef<DialogComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {
    this.config = data.config;
  }

  isConfirmDialog(): boolean {
    return this.config.dialogType === DialogType.Confirm;
  }

  isErrorDialog(): boolean {
    return this.config.dialogType === DialogType.Error;
  }
}
