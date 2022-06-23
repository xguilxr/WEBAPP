import {NgModule} from '@angular/core';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatMenuModule} from '@angular/material/menu';
import {LanguagesComponent} from 'app/layout/common/languages/languages.component';
import {SharedModule} from 'app/shared/shared.module';
import {ApiService} from 'app/services/api.service';

@NgModule({
    declarations: [LanguagesComponent],
    imports: [MatButtonModule, MatIconModule, MatMenuModule, SharedModule],
    exports: [LanguagesComponent],
    providers: [ApiService]
})
export class LanguagesModule {
}
