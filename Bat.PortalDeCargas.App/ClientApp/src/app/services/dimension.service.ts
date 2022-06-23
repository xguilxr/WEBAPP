import {Observable} from 'rxjs';
import {ApiService} from 'app/services/api.service';
import {Dimension} from 'app/model/dimension';
import {DimensionFilter} from 'app/model/dimension-filter';
import {PaginatedDimensions} from 'app/model/paginated-dimension';
import {DimensionFormDTO} from 'app/model/DimensionFormDTO';
import {ReturnCall} from 'app/model/ReturnCall';
import {Injectable, SkipSelf} from '@angular/core';
import {PaginatedEntity} from '../model/paginated-entity';
import {DimensionLog} from '../model/dimension-log';
import {DimensionLogFilter} from '../model/dimension-log-filter';
import {AuthSignInComponent} from "../modules/auth/sign-in/sign-in.component";

@Injectable()
export class DimensionService {
    constructor(@SkipSelf() private readonly api: ApiService) {
    }

    getAllDimensions(): Observable<Dimension[]> {
        return this.api.get('dimension/');
    }

    getDimensionById(dimensionId: string): Observable<Dimension> {
        return this.api.get<Dimension>(`dimension/GetDimensionById/${dimensionId}`);
    }

    getPaginated(filter: DimensionFilter): Observable<PaginatedDimensions> {
        return this.api.get(`dimension/filtered?${this.api.toQueryString(filter)}`);
    }

    save(dimensionForm: DimensionFormDTO): Observable<ReturnCall> {
        return this.api.post<ReturnCall>('dimension', dimensionForm);
    }

    delete(dimensionId: number): Observable<unknown> {
        return this.api.delete(`dimension/${dimensionId}`);
    }

    deleteDomain(dimensionId: number): Observable<unknown> {
        return this.api.delete(`dimension/deleteDomain/${dimensionId}`);
    }

    copy(dimensionId: number,
         dimensionName: string): Observable<ReturnCall> {
        return this.api.post<ReturnCall>('dimension/copy', {
            id: dimensionId,
            name: dimensionName
        });
    }

    uploadDimensionDomain(file: File,
                          dimensionId: number): Observable<ReturnCall> {
        const formData: FormData = new FormData();
        formData.append('file', file, file.name);
        formData.append('dimensionId', dimensionId.toString());
        return this.api.post<ReturnCall>('dimension/uploadDomain', formData);
    }

    uploadDimensionTest(file: File,
                        dimensionId: number): Observable<Blob> {
        const formData: FormData = new FormData();
        formData.append('file', file, file.name);
        formData.append('dimensionId', dimensionId.toString());
        return this.api.postblob('dimension/uploadTest', formData);
    }

    getExcelFile(name: string): Observable<Blob> {
        const formData: FormData = new FormData();
        formData.append('name', name);
        return this.api.postblob('dimension/getDimensionFile', formData);
    }

    getFilteredLog(filter: DimensionLogFilter): Observable<PaginatedEntity<DimensionLog>> {
        return this.api.get<PaginatedEntity<DimensionLog>>('dimension/getDimensionLog' + this.api.buildQueryString(filter));
    }

}
