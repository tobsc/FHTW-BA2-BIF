import {Routes, RouterModule} from "@angular/router";
import {DashboardComponent} from "./dashboard.component";
import {AuthGuard} from "../authentication/auth.guard";
const DASHBOARD_ROUTES: Routes = [
  { path: '', component: DashboardComponent, canActivate: [AuthGuard] }
];
export const dashboardRouting = RouterModule.forChild(DASHBOARD_ROUTES);
