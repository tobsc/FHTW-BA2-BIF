import {NgModule} from "@angular/core";
import {ModalComponent} from "./modal/modal.component";
import {CommonModule} from "@angular/common";
import {PanelCollapseDirective} from "./panel-collapse.directive";
import {SortPipe} from "../pipes/sort.pipe";
import {NavComponent} from "../nav.component";
import { ErrorMessageComponent } from './error-message/error-message.component';
import {RouterModule} from "@angular/router";
@NgModule({
  declarations: [
    ModalComponent,
    PanelCollapseDirective,
    SortPipe,
    NavComponent,
    ErrorMessageComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
  ],
  exports: [
    ErrorMessageComponent,
    ModalComponent,
    PanelCollapseDirective,
    SortPipe,
    NavComponent,
  ]
})
export class SharedModule {}