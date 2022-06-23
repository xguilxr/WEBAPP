import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseConfigService } from '@fuse/services/config';
import { BaseErrorComponent } from '../base-error.component';

@Component({
  selector: 'app-not-authorized',
  template: `
    <div class="mt-20 mb-40" fxLayout="column">
      <div class="header w-100-p mb-20">
        <img src="assets/images/logo/logo.svg" class="erro-img" />
      </div>

      <div class="erro-container" fxLayout="column" fxLayoutAlign="center center" fxLayoutGap="15px">
        <h1 class="medium-text">Não Autorizado!</h1>
        <p class="fuse-navy-200-fg large-text">
          <span>Você não tem permissão para acessar esta página.</span>
        </p>
      </div>
    </div>
  `,
  styleUrls: ['../error.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotAuthorizedComponent extends BaseErrorComponent {
  constructor(route: ActivatedRoute, router: Router, fuseConfig: FuseConfigService) {
    super(route, router, fuseConfig);
  }
}
