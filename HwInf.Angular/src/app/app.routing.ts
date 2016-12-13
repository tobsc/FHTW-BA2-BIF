import {Routes, RouterModule} from "@angular/router";
import {DashboardComponent} from "./dashboard/dashboard.component";
import {LoginComponent} from "./login/login.component";
import {AuthGuard} from "./shared/auth.guard";

const APP_ROUTES: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard]},
    { path: 'devices', canActivate: [AuthGuard], loadChildren: 'app/devices/devices.module#DevicesModule' },
];

export const routing = RouterModule.forRoot(APP_ROUTES);
