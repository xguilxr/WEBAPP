import { ActivatedRoute, Router } from '@angular/router';
import { OnInit, OnDestroy } from '@angular/core';
import { FuseConfigService } from '@fuse/services/config';
import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector     : 'BaseError',
  template: ''
  
})


export abstract class BaseErrorComponent implements OnInit, OnDestroy {
  canNavigateBack = window.history.length > 1;

  constructor(private readonly route: ActivatedRoute, private readonly router: Router, private readonly fuseConfig: FuseConfigService) {}

  ngOnInit(): void {
    this.setFuseConfig(true);
  }

  ngOnDestroy(): void {
    this.setFuseConfig(false);
  }

  back(): void {
    const returnUrl = this.route.snapshot.queryParams.returnUrl;

    if (returnUrl && returnUrl.trim().length) {
      this.router.navigateByUrl(decodeURIComponent(returnUrl), { replaceUrl: true });
    }

    this.router.navigate(["/"]);
  }

  
  private setFuseConfig(hidden: boolean): void {
   /* const config = this.fuseConfig.getConfig();

    if (config.layout.navbar) {
      config.layout.navbar.hidden = hidden;
    }

    this.fuseConfig.setConfig(config);*/
  }
}
