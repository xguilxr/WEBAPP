import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, CanActivateChild, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree} from '@angular/router';
import {Observable, of, switchMap} from 'rxjs';
import {AuthService} from 'app/core/auth/auth.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanActivateChild, CanLoad {
    /**
     * Constructor
     */
    constructor(private _authService: AuthService,
                private _router: Router) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
    /**
     * Can activate
     *
     * @param route
     * @param state
     */
    canActivate(route: ActivatedRouteSnapshot,
                state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        const redirectUrl = state.url === '/sign-out' ? '/' : state.url;
        const roules = this.getRoutePermissions(route);
        return this._check(redirectUrl, roules);
    }

    /**
     * Can activate child
     *
     * @param childRoute
     * @param state
     */
    canActivateChild(childRoute: ActivatedRouteSnapshot,
                     state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        const redirectUrl = state.url === '/sign-out' ? '/' : state.url;
        const roles = this.getRoutePermissions(childRoute);
        return this._check(redirectUrl, roles);
    }

    /**
     * Can load
     *
     * @param route
     * @param segments
     */
    canLoad(route: Route,
            segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
        return this._check('/', null);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------
    /**
     * Get allowed user roles from the route.
     * @param {ActivatedRouteSnapshot} route - The route.
     * @returns {string[]} All user roles that are allowed to access the route.
     */
    private getRoutePermissions(route: ActivatedRouteSnapshot): string[] {
        if (route.data && route.data.userRoles) {
            return route.data.userRoles as string[];
        }
        return null;
    }

    /**
     * Check the authenticated status
     *
     * @param redirectURL
     * @private
     */
    private _check(redirectURL: string,
                   roles: string[]): Observable<boolean> {
        // Check the authentication status
        return this._authService.check(roles)
        .pipe(switchMap((authenticated) => {
            // If the user is not authenticated...
            if (!authenticated) {
                // Redirect to the sign-in page
                this._router.navigate(['sign-in'], {queryParams: {redirectURL}});
                // Prevent the access
                return of(false);
            }
            // Allow the access
            return of(true);
        }));
    }
}
