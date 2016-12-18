import {Routes, RouterModule} from "@angular/router";
import {DashboardComponent} from "./dashboard.component";
import {AuthGuard} from "../shared/auth.guard";
const DASHBOARD_ROUTES: Routes = [
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard],}
];
export const dashboardRouting = RouterModule.forChild(DASHBOARD_ROUTES);
