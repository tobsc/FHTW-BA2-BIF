import {NgModule} from "@angular/core";
import {NavComponent} from "./nav/nav.component";
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";
import {SharedModule} from "../shared/shared.module";
import {DropdownDirective} from "../shared/bootstrap/dropdown.directive";
@NgModule({
  declarations: [
    NavComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule
  ],
  exports: [
    NavComponent,
  ]
})
export class CoreModule {}
