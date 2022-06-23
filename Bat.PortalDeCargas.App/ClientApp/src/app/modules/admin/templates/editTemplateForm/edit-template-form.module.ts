import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { EditTemplateFormComponent } from './edit-template-form.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatRadioModule } from '@angular/material/radio';
import { SharedModule } from 'app/shared/shared.module';
import {TemplateService} from '../../../../services/template.service';
import {NgxDatatableModule} from "@swimlane/ngx-datatable";
import {DimensionService} from "../../../../services/dimension.service";

const editTemplatesFormRoutes: Route[] = [
    {
        path     : '',
        component: EditTemplateFormComponent
    }
];

@NgModule({
    declarations: [
        EditTemplateFormComponent
    ],
    imports: [
        RouterModule.forChild(editTemplatesFormRoutes),
        MatFormFieldModule,
        MatOptionModule,
        MatSelectModule,
        MatInputModule,
        MatRadioModule,
        MatDatepickerModule,
        MatTabsModule,
        MatTableModule,
        MatTabsModule,
        MatTableModule,
        SharedModule,
        NgxDatatableModule
    ],
    providers: [TemplateService, DimensionService]
})
export class EditTemplateFormModule
{
}
