import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { DimensoesComponent } from 'app/modules/admin/dimensoes/dimensoes.component';
import { DimensionService } from 'app/services/dimension.service';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { SharedModule } from 'app/shared/shared.module';
import { CopiarDimensoesDialogComponent } from './copiarDimensoesDialog/copiarDimensoesDialog.component';


const dimensoesRoutes: Route[] = [
    {
        path     : '',
        component: DimensoesComponent
    }
];

@NgModule({
    declarations: [
        DimensoesComponent,
        CopiarDimensoesDialogComponent
    ],
    imports     : [
        RouterModule.forChild(dimensoesRoutes),
        NgxDatatableModule,
        SharedModule
        
    ],
    
    providers: [DimensionService]
})
export class DimensoesModule
{
}
