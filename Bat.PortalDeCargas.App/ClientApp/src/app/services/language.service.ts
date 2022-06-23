import { Injectable, SkipSelf } from '@angular/core';
import { ApiService } from 'app/services/api.service';

@Injectable()
export class LanguageService {
    constructor(@SkipSelf() private readonly api: ApiService) { }


    setLanguage(culture : string){
        return this.api.post('language',{ culture: culture});
      }
}