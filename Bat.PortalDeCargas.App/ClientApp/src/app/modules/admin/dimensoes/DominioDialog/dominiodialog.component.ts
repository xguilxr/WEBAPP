import {Component, Inject, ViewEncapsulation} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {dimensionDomain} from 'app/model/DimensionDomain';

export interface domainValue {
    domains: dimensionDomain[]
}

@Component({
    selector: 'dominioDialog',
    templateUrl: './dominioDialog.component.html',
    encapsulation: ViewEncapsulation.None
})


export class DominioDialogComponent {
    /**
     * Constructor
     */
    constructor(
        public dialogRef: MatDialogRef<DominioDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: domainValue,
    ) {
    }

    onClick(): void {
        this.dialogRef.close();
    }
}
