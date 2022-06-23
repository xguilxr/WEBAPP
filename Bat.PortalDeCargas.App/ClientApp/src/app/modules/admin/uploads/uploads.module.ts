import {NgModule} from '@angular/core';
import {Route, RouterModule} from '@angular/router';
import {UploadsComponent} from 'app/modules/admin/uploads/uploads.component';
import {SharedModule} from '../../../shared/shared.module';
import {NgxDatatableModule} from '@swimlane/ngx-datatable';
import {TemplateService} from '../../../services/template.service';

const uploadsRoutes: Route[] = [{
    path: '',
    component: UploadsComponent
}];

@NgModule({
    declarations: [UploadsComponent],
    imports: [RouterModule.forChild(uploadsRoutes), SharedModule, NgxDatatableModule],
    providers: [TemplateService]
})
export class UploadsdModule {
}
