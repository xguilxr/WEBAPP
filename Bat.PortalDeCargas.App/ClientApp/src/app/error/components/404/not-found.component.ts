import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseConfigService } from '@fuse/services/config';
import { BaseErrorComponent } from '../base-error.component';

@Component({
  selector: 'app-not-found',
  template: `
    <div class="flex flex-row">
    <div class ="basis-1/4">
       &nbsp;
    </div>
    <div class ="basis-1/2">
    <div class="mt-20 mb-40" fxLayout="column">
      <div class="header w-100-p mb-20">
        <img src="assets/images/logo/logo.svg" />
      </div>

      <div class="erro-container" fxLayout="column" fxLayoutAlign="center center" fxLayoutGap="15px">
        <h1 class="large-text">404</h1>
        <p class="fuse-navy-200-fg large-text">
          <span>A página que você procura não foi encontrada.</span>
        </p>
        <button *ngIf="canNavigateBack" type="button" mat-raised-button color="primary" (click)="back()">RETORNAR</button>
      </div>
    </div>
    </div>
    <div class ="basis-1/4">
       &nbsp;
    </div>
    </div>
  `,
  styleUrls: ['../error.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotFoundComponent extends BaseErrorComponent {
  constructor(route: ActivatedRoute, router: Router, fuseConfig: FuseConfigService) {
    super(route, router, fuseConfig);
  }
}
