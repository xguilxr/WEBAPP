import { ComponentType } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';

import { DialogComponent } from '../components/dialog/dialog.component';
import { DialogConfig, DialogType } from '../model/dialog';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class DialogService {
  constructor(private readonly dialog: MatDialog) {}

  private openDialog(config: DialogConfig): Observable<any> {
    const dialogRef = this.dialog.open(DialogComponent, { data: { config } });

    return dialogRef.afterClosed();
  }

  fromComponent<T, D>(component: ComponentType<T>, config?: MatDialogConfig<D>): MatDialogRef<T> {
    return this.dialog.open(component, config);
  }

  openInfoDialog(title: string, content: string, okButtonText: string = 'OK'): Observable<any> {
    return this.openDialog({ dialogType: DialogType.Info, title, content, okButtonText });
  }

  openConfirmDialog(content: string, title?: string, okButtonText: string = 'OK', cancelButtonText: string = 'CANCELAR'): Observable<any> {
    return this.openDialog({ dialogType: DialogType.Confirm, title, content, okButtonText, cancelButtonText });
  }

  openErrorDialog(title: string, content: string, okButtonText: string = 'OK'): Observable<any> {
    return this.openDialog({ dialogType: DialogType.Error, title, content, okButtonText });
  }

  openHttpErrorDialog(response: HttpErrorResponse): Observable<any> {
    let title = null;
    let content = '';
    if (response instanceof Error) {
      console.error(response);
      return this.openErrorDialog(title, 'Um erro desconhecido ocorreu');
    }

    if (!response.error) content = `${response.status} - Um erro desconhecido ocorreu`;
    else {
      if (response.error.title) {
        title = response.error.title;
        content = `${response.message}<br />`;
      }

      if (response.error.error) content += response.error.error;
      else if (response.error.message) content += response.error.message;
      else if (Array.isArray(response.error)) content += response.error.join('<br />');
      else if (response.error.errors) {
        content += Object.keys(response.error.errors)
          .map(k => `${k}: ${response.error.errors[k].join('<br />')}`)
          .join('<br />');
      } else if (typeof response.error === 'object') {
        content += Object.keys(response.error)
          .map(k => response.error[k].join('<br />'))
          .join('<br />');
      } else if (typeof response.error === 'string') {
        content = response.error;
      }
    }

    return this.openErrorDialog(title, content);
  }
}
