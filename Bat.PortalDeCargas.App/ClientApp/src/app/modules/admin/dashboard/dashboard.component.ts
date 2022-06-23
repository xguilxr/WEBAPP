import {Component, ViewEncapsulation} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from "../../../core/auth/auth.service";

@Component({
    selector: 'dashboard',
    templateUrl: './dashboard.component.html',
    encapsulation: ViewEncapsulation.None
})
export class DashboardComponent {
    constructor(private router: Router,
                private _authService: AuthService) {
    }

    goToUploads(): void {
        this.router.navigate(['uploads']);
    }

    goToDimensoes(): void {
        this.router.navigate(['dimensoes']);
    }

    goToTemplates(): void {
        this.router.navigate(['templates']);
    }

    goToGerenciamento(): void {
        this.router.navigate(['gerenciamento']);
    }

    public showBox(roles: string[]): boolean {
        return this._authService.areUserRolesAllowed(roles);
    }
}
