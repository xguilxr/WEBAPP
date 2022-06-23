import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';

import { NotFoundComponent } from './components/404/not-found.component';
import { ErrorComponent } from './components/generic/error.component';
import { NotAuthorizedComponent } from './components/not-authorized/not-authorized.componen';

@NgModule({
  declarations: [ErrorComponent, NotFoundComponent, NotAuthorizedComponent],
  imports: [CommonModule, MatButtonModule, FlexLayoutModule]
})
export class ErrorModule {}
