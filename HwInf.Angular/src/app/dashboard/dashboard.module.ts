import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {DashboardComponent} from "./dashboard.component";
import {dashboardRouting} from "./dashboard.routing";
import {SharedModule} from "../shared/shared.module";

@NgModule({
  declarations: [
    DashboardComponent
  ],
  imports: [
    CommonModule,
    dashboardRouting
  ],
})
export class DashboardModule { }
