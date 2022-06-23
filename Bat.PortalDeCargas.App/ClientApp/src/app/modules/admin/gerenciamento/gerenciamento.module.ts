import {NgModule} from '@angular/core';
import {Route, RouterModule} from '@angular/router';
import {GerenciamentoComponent} from 'app/modules/admin/gerenciamento/gerenciamento.component';
import {MatTabsModule} from '@angular/material/tabs';
import {MatTableModule} from '@angular/material/table';
import {SharedModule} from '../../../shared/shared.module';
import {DimensionsReportComponent} from './dimensions-report/dimensions-report.component';
import {NgxDatatableModule} from '@swimlane/ngx-datatable';
import {DimensionService} from '../../../services/dimension.service';
import {TemplateReportComponent} from './template-report/template-report.component';
import {TemplateService} from '../../../services/template.service';
import {UploadReportComponent} from './upload-report/upload-report.component';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatMomentDateModule} from '@angular/material-moment-adapter';

const gerenciamentoRoutes: Route[] = [{
    path: '',
    component: GerenciamentoComponent
}];

@NgModule({
    declarations: [GerenciamentoComponent, DimensionsReportComponent, TemplateReportComponent, UploadReportComponent],
    providers: [DimensionService, TemplateService],
    imports: [RouterModule.forChild(gerenciamentoRoutes), MatTabsModule, MatTableModule, SharedModule, NgxDatatableModule, MatDatepickerModule, MatMomentDateModule]
})
export class GerenciamentoModule {
}
