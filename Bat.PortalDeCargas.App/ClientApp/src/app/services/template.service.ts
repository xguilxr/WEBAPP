import {Injectable, SkipSelf} from '@angular/core';
import {Observable} from 'rxjs';
import {ApiService} from 'app/services/api.service';
import {Template} from '../model/template';
import {QueryFilter} from '../model/QueryFilter';
import {PaginatedEntity} from '../model/paginated-entity';
import {TemplateFormDTO} from '../model/templateFormDTO';
import {ReturnCall} from '../model/ReturnCall';
import {UploadResult} from '../model/uploadResult';
import {TemplateLogFilter} from '../model/template-log-filter';
import {TemplateLog} from '../model/template-log';
import {UploadLogFilter} from '../model/upload-log-filter';
import {FileDetail} from '../model/fileDetail';
import {UploadLog} from '../model/upload-log';
import {FileDetailFilter} from "../model/file-detail-filter";

@Injectable()
export class TemplateService {
    constructor(@SkipSelf() private readonly api: ApiService) {
    }

    getPaginated(filter: QueryFilter): Observable<PaginatedEntity<Template>> {
        return this.api.get(`template/filtered?${this.api.toQueryString(filter)}`);
    }

    getTemplateById(templateId: number): Observable<Template> {
        return this.api.get<Template>(`template/GetTemplateById/${templateId}`);
    }

    save(templateForm: TemplateFormDTO): Observable<ReturnCall> {
        return this.api.post<ReturnCall>('template', templateForm);
    }

    delete(templateId: number): Observable<any> {
        return this.api.delete(`template/${templateId}`);
    }

    getFileTemplate(templateId: number): Observable<Blob> {
        return this.api.postblob(`template/${templateId}/getFile`, {});
    }

    uploadFile(file: File,
               templateId: number): Observable<Blob> {
        const formData: FormData = new FormData();
        formData.append('file', file, file.name);
        return this.api.postblob(`template/${templateId}/validateFile`, formData);
    }

    getUploadResult(templateId: number): Observable<UploadResult> {
        return this.api.get<UploadResult>(`template/upload/${templateId}`);
    }

    getFilteredLog(filter: TemplateLogFilter): Observable<PaginatedEntity<TemplateLog>> {
        return this.api.get<PaginatedEntity<TemplateLog>>('template/getTemplateLog' + this.api.buildQueryString(filter));
    }

    getFilteredUpdateLog(filter: UploadLogFilter): Observable<PaginatedEntity<UploadLog>> {
        return this.api.get<PaginatedEntity<UploadLog>>('template/getUploadLog' + this.api.buildQueryString(filter));
    }

    getUploadDetail(filter: FileDetailFilter): Observable<PaginatedEntity<FileDetail>> {
        return this.api.get<PaginatedEntity<FileDetail>>('template/upload/detail' + this.api.buildQueryString(filter));
    }
}
