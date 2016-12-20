import {NgModule} from "@angular/core";
import {ModalComponent} from "./modal/modal.component";
import {CommonModule} from "@angular/common";
import {PanelCollapseDirective} from "./bootstrap/panel-collapse.directive";
import {SortPipe} from "./pipes/sort.pipe";
import {NavComponent} from "../nav.component";
import { ErrorMessageComponent } from './error-message/error-message.component';
import {RouterModule} from "@angular/router";
import { DropdownDirective } from './bootstrap/dropdown.directive';
@NgModule({
  declarations: [
    ModalComponent,
    PanelCollapseDirective,
    SortPipe,
    NavComponent,
    ErrorMessageComponent,
    DropdownDirective
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
