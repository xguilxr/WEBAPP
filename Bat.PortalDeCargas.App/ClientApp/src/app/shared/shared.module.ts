import { LanguageService } from './../services/language.service';
import { ErrorModule } from './../error/error.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslocoModule } from '@ngneat/transloco';
import { DialogComponent } from 'app/components/dialog/dialog.component';
import { DialogService } from 'app/services/dialog.service';
import {MatDialogModule} from '@angular/material/dialog';
import {MatChipsModule} from '@angular/material/chips';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { UsersService } from 'app/services/users.service';
import { MatRadioModule } from '@angular/material/radio';



@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MatTableModule,
        MatButtonModule,
        MatIconModule,
        MatFormFieldModule,
        MatInputModule,
        TranslocoModule,
        MatDialogModule,
        ErrorModule,
        MatChipsModule,
        MatAutocompleteModule,
        MatSnackBarModule,
        MatRadioModule
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MatTableModule,
        MatButtonModule,
        MatIconModule,
        MatFormFieldModule,
        MatInputModule,
        TranslocoModule,
        MatDialogModule,
        ErrorModule,
        MatChipsModule,
        MatAutocompleteModule,
        MatSnackBarModule,
        MatRadioModule
    ],
    declarations: [DialogComponent],
    providers: [DialogService,LanguageService,UsersService]
})
export class SharedModule
{
}
