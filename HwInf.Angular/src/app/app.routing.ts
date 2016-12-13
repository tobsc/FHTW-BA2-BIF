import {Routes, RouterModule} from "@angular/router";
import {DevicesComponent} from "./devices/devices.component";
import {DashboardComponent} from "./dashboard/dashboard.component";
import {DEVICES_ROUTES} from "./devices/devices.routing";
import {LoginComponent} from "./login/login.component";
import {AuthGuard} from "./shared/auth.guard";

const APP_ROUTES: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard]},
    { path: 'devices', component: DevicesComponent, canActivate: [AuthGuard], children: DEVICES_ROUTES },
];

export const routing = RouterModule.forRoot(APP_ROUTES);
