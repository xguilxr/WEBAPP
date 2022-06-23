import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { CargaComponent } from 'app/modules/admin/carga/carga.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';

const cargaRoutes: Route[] = [
    {
        path     : '',
        component: CargaComponent
    }
];

@NgModule({
    declarations: [
        CargaComponent
    ],
    imports     : [
        RouterModule.forChild(cargaRoutes),
        MatTabsModule,
        MatTableModule
    ]
})
export class CargaModule
{
}
