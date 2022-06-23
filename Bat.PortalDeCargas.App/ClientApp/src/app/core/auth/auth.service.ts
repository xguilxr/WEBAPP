import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {catchError, Observable, of, switchMap, throwError} from 'rxjs';
import {AuthUtils} from 'app/core/auth/auth.utils';

@Injectable()
export class AuthService {
    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    get authenticated(): boolean {
        return this.accessToken !== '';
    }

    get accessToken(): string {
        const token = localStorage.getItem('accessToken');
        if (token === undefined || token === null || token === 'undefined') {
            return '';
        }
        return token;
    }

    /**
     * Setter & getter for access token
     */
    set accessToken(token: string) {
        localStorage.setItem('accessToken', token);
    }

    get role(): string {
        const userRole = localStorage.getItem('userRole');
        if (userRole === undefined || userRole === null || userRole === 'undefined') {
            return '';
        }
        return userRole;
    }

    set role(userRole: string) {
        localStorage.removeItem('userRole');
        localStorage.setItem('userRole', userRole);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
    /**
     * Forgot password
     *
     * @param email
     */
    /**
     * Sign in
     *
     * @param credentials
     */
    signIn(credentials: { email: string; password: string }): Observable<any> {
        // Throw error, if the user is already logged in
        if (this.authenticated) {
            return throwError(() => 'User is already logged in.');
        }
        return this._httpClient.post('api/auth/login', credentials).pipe(switchMap((response: any) => {
            // Store the access token in the local storage
            this.accessToken = response.token;
            // Store the user on the user service
            this.role = response.role;
            // Return a new observable with the response
            return of(response);
        }));
    }

    /**
     * Sign in using the access token
     */
    signInUsingToken(): Observable<any> {
        // Renew token
        return this._httpClient.post('api/auth/refreshToken', {
            accessToken: this.accessToken
        }).pipe(catchError(() => // Return false
            of(false)), switchMap((response: any) => {
            // Store the access token in the local storage
            this.accessToken = response.token;
            this.role = response.role;
            // Return true
            return of(true);
        }));
    }

    /**
     * Sign out
     */
    signOut(): Observable<any> {
        // Remove the access token from the local storage
        localStorage.removeItem('accessToken');
        localStorage.removeItem('userRole');
        // Return the observable
        return of(true);
    }

    /**
     * Check the authentication status
     */
    check(allowedUserRoles: string[]): Observable<boolean> {
        // Check if the user is logged in
        if (this.authenticated) {
            if (this.areUserRolesAllowed(allowedUserRoles)) {
                return of(true);
            } else {
                return of(false);
            }
        }
        // Check the access token availability
        if (!this.accessToken) {
            return of(false);
        }
        // Check the access token expire date
        if (AuthUtils.isTokenExpired(this.accessToken)) {
            return of(false);
        }
        // If the access token exists and it didn't expire, sign in using it
        return this.signInUsingToken();
    }

    resetPassword(value: any): any {
        return null;
    }

    forgotPassword(value: any): any {
        return null;
    }

    public areUserRolesAllowed(allowedUserRoles: string[]): boolean {
        if (!allowedUserRoles || allowedUserRoles.length === 0) {
            return true;
        }
        for (const allowedRole of allowedUserRoles) {
            if (this.role.toLowerCase() === allowedRole.toLowerCase()) {
                return true;
            }
        }
        return false;
    }
}
