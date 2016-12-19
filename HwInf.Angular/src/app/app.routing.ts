import {Routes, RouterModule} from "@angular/router";
import {DashboardComponent} from "./dashboard/dashboard.component";
import {LoginComponent} from "./login/login.component";
import {AuthGuard} from "./shared/auth.guard";
import {CartComponent} from "./cart/cart.component";

const APP_ROUTES: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', loadChildren: 'app/dashboard/dashboard.module#DashboardModule'},
    { path: 'devices', loadChildren: 'app/devices/devices.module#DevicesModule' },
    { path: 'cart', loadChildren: 'app/cart/cart.module#CartModule' },

];

export const routing = RouterModule.forRoot(APP_ROUTES);
