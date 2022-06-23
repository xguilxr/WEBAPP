import { UsersService } from './../../../../services/users.service';
import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { SharedModule } from './../../../../shared/shared.module';
import { CadastroUsuariosComponent } from './cadastrousuarios.component';
import { TrocaSenhaDialogComponent } from '../trocaSenhaDialog/trocaSenhaDialog.component';



const CadastroUsuariosRoutes: Route[] = [
    {
        path     : '',
        component: CadastroUsuariosComponent
    }
];

@NgModule({
    declarations: [
        CadastroUsuariosComponent
       
    ],
    imports     : [
        RouterModule.forChild(CadastroUsuariosRoutes),        
        SharedModule        
    ],
    
    providers: [UsersService]
})

export class CadastroUsuariosModule
{
}
