import {Routes, RouterModule} from "@angular/router";
import {LoginComponent} from "./authentication/login.component";
import {PageNotFoundComponent} from "./page-not-found/page-not-found.component";

const APP_ROUTES: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', loadChildren: './dashboard/dashboard.module#DashboardModule'},
    { path: 'devices', loadChildren: './devices/devices.module#DevicesModule' },
    { path: 'cart', loadChildren: './cart/cart.module#CartModule' },
    { path: 'orders', loadChildren: './order/order.module#OrderModule' },
    { path: 'admin', loadChildren: './admin/admin.module#AdminModule' },
    { path: '**', component: PageNotFoundComponent }

];

export const routing = RouterModule.forRoot(APP_ROUTES);
