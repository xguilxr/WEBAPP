import {NgModule} from '@angular/core';
import {Route, RouterModule} from '@angular/router';
import {UploadComponent} from './upload.component';
import {MatTabsModule} from '@angular/material/tabs';
import {MatTableModule} from '@angular/material/table';
import {TemplateService} from '../../../../services/template.service';
import {SharedModule} from '../../../../shared/shared.module';
import {MatToolbarModule} from '@angular/material/toolbar';
import {NgxDatatableModule} from '@swimlane/ngx-datatable';

const uploadRoutes: Route[] = [{
    path: '',
    component: UploadComponent
}];

@NgModule({
    declarations: [UploadComponent],
    imports: [RouterModule.forChild(uploadRoutes), MatTabsModule, MatTableModule, SharedModule, MatToolbarModule, NgxDatatableModule],
    providers: [TemplateService]
})
export class UploadModule {
}
