import {NgModule} from '@angular/core';
import {TemplatesComponent} from './templates.component';
import {Route, RouterModule} from '@angular/router';
import {NgxDatatableModule} from '@swimlane/ngx-datatable';
import {TemplateService} from 'app/services/template.service';
import { SharedModule } from 'app/shared/shared.module';

const templatesRoutes: Route[] = [
    {
        path: '',
        component: TemplatesComponent
    }
];

@NgModule({
    declarations: [
        TemplatesComponent
    ],
    imports: [
        RouterModule.forChild(templatesRoutes),
        NgxDatatableModule,
        SharedModule
    ],
    providers: [TemplateService]
})
export class TemplatesModule {
}
