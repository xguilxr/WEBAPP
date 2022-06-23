import { UsersService } from './../../../services/users.service';
import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { SharedModule } from 'app/shared/shared.module';
import { UsuariosComponent } from './usuarios.component';
import { TrocaSenhaDialogComponent } from './trocaSenhaDialog/trocaSenhaDialog.component';



const UsuariosRoutes: Route[] = [
    {
        path     : '',
        component: UsuariosComponent
    }
];

@NgModule({
    declarations: [
        UsuariosComponent,
        TrocaSenhaDialogComponent       
    ],
    imports     : [
        RouterModule.forChild(UsuariosRoutes),
        NgxDatatableModule,
        SharedModule        
    ],
    
    providers: [UsersService]
})
export class UsuariosModule
{
}
