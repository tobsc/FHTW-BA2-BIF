import { Routes, RouterModule } from "@angular/router";
import { PageNotFoundComponent } from "./core/page-not-found/page-not-found.component";
import { LoginComponent } from "./authentication/login.component";
import { DashboardComponent } from "./user/dashboard/dashboard.component";
import {   AuthGuard } from "./authentication/auth.guard";
import {HomeComponent} from "./home/home.component";
import {DeviceListComponent} from "./user/devices/device-list.component";

const APP_ROUTES: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard],
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
            { path: 'geraete', component: DeviceListComponent, canActivate: [AuthGuard] },
            { path: 'geraete/typ/:type', component: DeviceListComponent, canActivate: [AuthGuard] },
        ]
    },
    { path: 'login', component: LoginComponent },
    { path: '**', component: PageNotFoundComponent }
];

export const routing = RouterModule.forRoot(APP_ROUTES);
