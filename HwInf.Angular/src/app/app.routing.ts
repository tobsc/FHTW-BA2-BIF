import {Routes, RouterModule} from "@angular/router";
import {DEVICES_ROUTES} from "./devices/devices.routes";
import {DevicesComponent} from "./devices/devices.component";
import {DashboardComponent} from "./dashboard/dashboard.component";

const APP_ROUTES: Routes = [
    { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
    { path: 'dashboard', component: DashboardComponent},
    { path: 'devices', component: DevicesComponent, children: DEVICES_ROUTES },
];

export const routing = RouterModule.forRoot(APP_ROUTES);
