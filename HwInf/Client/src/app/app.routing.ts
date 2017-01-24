import {Routes, RouterModule} from "@angular/router";
import {LoginComponent} from "./authentication/login.component";
import {PageNotFoundComponent} from "./page-not-found/page-not-found.component";

const APP_ROUTES: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', loadChildren: 'app/dashboard/dashboard.module#DashboardModule'},
    { path: 'devices', loadChildren: 'app/devices/devices.module#DevicesModule' },
    { path: 'cart', loadChildren: 'app/cart/cart.module#CartModule' },
    { path: 'orders', loadChildren: 'app/order/order.module#OrderModule' },
    { path: 'admin', loadChildren: 'app/admin/admin.module#AdminModule' },
    { path: '**', component: PageNotFoundComponent }

];

export const routing = RouterModule.forRoot(APP_ROUTES);
