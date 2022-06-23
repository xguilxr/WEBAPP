import {UsersFilter} from './../model/users.filter';
import {Injectable, SkipSelf} from '@angular/core';
import {Observable} from 'rxjs';
import {ApiService} from 'app/services/api.service';
import {ReturnCall} from 'app/model/ReturnCall';
import {Paginated} from 'app/model/paginated';
import {AppUser, ChangePassword} from 'app/model/users';

@Injectable()
export class UsersService {
    constructor(@SkipSelf() private readonly api: ApiService) {
    }

    getPaginated(filter: UsersFilter): Observable<Paginated<AppUser>> {
        return this.api.get(`Users/filtered?${this.toQueryString(filter)}`);
    }

    save(user: AppUser) {
        return this.api.post<ReturnCall>('Users/save', user);
    }

    delete(userId: number) {
        return this.api.delete(`users/${userId}`);
    }

    getUserById(userId: string) {
        return this.api.get<AppUser>(`users/GetUserById/${userId}`);
    }

    changePassword(password: ChangePassword) {
        return this.api.post<ReturnCall>('Users/changePassword', password);
    }

    private toQueryString(obj: any): string {
        return Object.keys(obj)
        .map(key => `${key}=${obj[key] === undefined || obj[key] === null ? '' : obj[key]}`)
        .join('&');
    }
}
