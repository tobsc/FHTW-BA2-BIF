import {NgModule} from "@angular/core";
import {ModalComponent} from "./modal/modal.component";
import {CommonModule} from "@angular/common";
import {PanelCollapseDirective} from "./panel-collapse.directive";
import {SortPipe} from "../pipes/sort.pipe";
import {NavComponent} from "../nav.component";
@NgModule({
  declarations: [
    ModalComponent,
    PanelCollapseDirective,
    SortPipe,
    NavComponent
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    ModalComponent,
    PanelCollapseDirective,
    SortPipe,
    NavComponent
  ]
})
export class SharedModule {}
