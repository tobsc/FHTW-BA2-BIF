import {Routes, RouterModule} from "@angular/router";
import {DashboardComponent} from "./dashboard/dashboard.component";
import {LoginComponent} from "./authentication/login.component";
import {AuthGuard} from "./authentication/auth.guard";
import {CartComponent} from "./cart/cart.component";
import {PageNotFoundComponent} from "./page-not-found/page-not-found.component";

const APP_ROUTES: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', loadChildren: 'app/dashboard/dashboard.module#DashboardModule'},
    { path: 'devices', loadChildren: 'app/devices/devices.module#DevicesModule' },
    { path: 'cart', loadChildren: 'app/cart/cart.module#CartModule' },
    { path: 'orders', loadChildren: 'app/order/order.module#OrderModule'},
    { path: '**', component: PageNotFoundComponent }

];

export const routing = RouterModule.forRoot(APP_ROUTES);
