import { DimensionService } from '../../../../services/dimension.service';
import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { CriarDimensoesComponent } from './criarDimensoes.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { SharedModule } from 'app/shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { DominioDialogComponent } from '../DominioDialog/dominiodialog.component';

const criarDimensoesRoutes: Route[] = [
    {
        path     : '',
        component: CriarDimensoesComponent
    }
];

@NgModule({
    declarations: [
        CriarDimensoesComponent,
        DominioDialogComponent
    ],
    imports     : [
        RouterModule.forChild(criarDimensoesRoutes),
        MatFormFieldModule,
        MatOptionModule,
        MatSelectModule,
        MatInputModule,
        MatRadioModule,
        MatDatepickerModule,
        MatTabsModule,
        MatTableModule,
        SharedModule,
        ReactiveFormsModule
    ],
    providers: [DimensionService]
})
export class CriarDimensoesModule
{
}
