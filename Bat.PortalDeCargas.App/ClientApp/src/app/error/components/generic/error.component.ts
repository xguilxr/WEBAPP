import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseConfigService } from '@fuse/services/config';
import { BaseErrorComponent } from '../base-error.component';

@Component({
  selector: 'app-error',
  template: `
    <div class="mt-20 mb-40" fxLayout="column">
      <div class="header w-100-p mb-20">
        <img src="assets/images/logo/logo.svg" />
      </div>
      <div class="erro-container" fxLayout="column" fxLayoutAlign="center center" fxLayoutGap="15px">
        <h1>Ocorreu um erro durante a exibição desta página!</h1>
        <p class="fuse-navy-200-fg h2">
          <span>Tente novamente mais tarde.</span>
        </p>
        <button *ngIf="canNavigateBack" type="button" mat-raised-button color="primary" (click)="back()">RETORNAR</button>
      </div>
    </div>
  `,
  styleUrls: ['../error.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ErrorComponent extends BaseErrorComponent {
  constructor(route: ActivatedRoute, router: Router, fuseConfig: FuseConfigService) {
    super(route, router, fuseConfig);
  }
}
