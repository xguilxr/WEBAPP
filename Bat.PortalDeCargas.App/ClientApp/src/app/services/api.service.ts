import {HttpClient, HttpErrorResponse, HttpHeaders, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import * as moment from 'moment';
import {Observable, throwError} from 'rxjs';
import {catchError} from 'rxjs/operators';
import {environment} from 'environments/environment';
import {Router} from '@angular/router';

type Headers = HttpHeaders | { [header: string]: string | string[] };
type Params = HttpParams | { [param: string]: string | string[] };

interface Options {
    headers?: Headers;
    observe?: 'body';
    params?: Params;
    reportProgress?: boolean;
    responseType?: 'json';
    withCredentials?: boolean;
}

@Injectable()
export class ApiService {
    constructor(private router: Router,
                private readonly httpClient: HttpClient) {
    }

    get<T>(url: string,
           options?: Options): Observable<T> {
        return this.httpClient.get<T>(this.resolveUrl(url), options).pipe(catchError(this.catchAuthError));
    }

    patch<T>(url: string,
             body: any,
             options?: Options): Observable<T> {
        return this.httpClient.patch<T>(this.resolveUrl(url), body, options).pipe(catchError(this.catchAuthError));
    }

    post<T>(url: string,
            body: any,
            options?: Options): Observable<T> {
        return this.httpClient.post<T>(this.resolveUrl(url), body, options).pipe(catchError(this.catchAuthError));
    }

    put<T>(url: string,
           body: any,
           options?: Options): Observable<T> {
        return this.httpClient.put<T>(this.resolveUrl(url), body, options).pipe(catchError(this.catchAuthError));
    }

    delete<T>(url: string,
              options?: Options): Observable<T> {
        return this.httpClient.delete<T>(this.resolveUrl(url), options).pipe(catchError(this.catchAuthError));
    }

    resolveUrl(path: string): string {
        return `${environment.apiUrl}api/${path}`;
    }

    postblob(url: string,
             body: any): Observable<Blob> {
        return this.httpClient.post(this.resolveUrl(url), body, {
            reportProgress: true,
            responseType: 'blob'
        }).pipe(catchError(this.catchAuthError));
    }

    getString(url: string): Observable<string> {
        return this.httpClient.get(this.resolveUrl(url), {responseType: 'text'}).pipe(catchError(this.catchAuthError));
    }

    public toQueryString(obj: any): string {
        return Object.keys(obj)
        .map(key => `${key}=${obj[key] === undefined || obj[key] === null ? '' : obj[key]}`)
        .join('&');
    }

    protected getDateParam(date: Date | moment.Moment): string {
        return (moment.isDate(date) ? moment(date) : date).format('YYYY-MM-DD');
    }

    public buildQueryString(data: any): string {
        let result = '?';
        Object.keys(data).forEach((value,
                                   idx,
                                   arr) => {
            const propertyValue = (data[value] === null || data[value] === undefined) ? '' : data[value];
            return result += value + `=${encodeURIComponent(propertyValue)}&`;
        });
        return result;
    }

    private catchAuthError = (res: HttpErrorResponse): Observable<never> => {
        if (res.status === 404) {
            this.router.navigate(['error/not-found'], {replaceUrl: false});
        } else if (res.status === 401 || res.status === 403) {
            this.router.navigate(['error/not-found'], {replaceUrl: false});
        } else if (res.status >= 500) {
            const queryParams = {returnUrl: encodeURIComponent(window.location.pathname)};
            this.router.navigate(['error'], {queryParams});
            return throwError(new HttpErrorResponse({error: 'Ocorreu um erro no sistema. Contate o administrador.'}));
        } else if (res.status === 0) {
            window.location.href = '/';
        }
        return throwError(res);
    };
}
