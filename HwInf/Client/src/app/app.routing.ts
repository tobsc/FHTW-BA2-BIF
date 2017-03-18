import { Routes, RouterModule } from "@angular/router";
import { PageNotFoundComponent } from "./core/page-not-found/page-not-found.component";
import { LoginComponent } from "./authentication/login.component";
import { DashboardComponent } from "./user/dashboard/dashboard.component";
import {   AuthGuard } from "./authentication/auth.guard";
import {HomeComponent} from "./home/home.component";
import { DeviceListComponent } from "./user/devices/device-list.component";
import { CartComponent } from "./user/cart/cart.component";
import { OrderStep1Component } from "./user/cart/order-step1/order-step1.component";
import { OrderStep2Component } from "./user/cart/order-step2/order-step2.component";
import { OrderComponent } from "./user/cart/order.component";

const APP_ROUTES: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard],
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
            { path: 'geraete', component: DeviceListComponent, canActivate: [AuthGuard] },
            { path: 'geraete/typ/:type', component: DeviceListComponent, canActivate: [AuthGuard] },
            { path: 'warenkorb', component: CartComponent, canActivate: [AuthGuard], },
            { path: 'anfrage', component: OrderComponent, canActivate: [AuthGuard], 
                children: [{ path: '', redirectTo: 'schritt-1', pathMatch: 'full' },
                            { path: 'schritt-1', component: OrderStep1Component, canActivate: [AuthGuard] },
                            { path: 'schritt-2', component: OrderStep2Component, canActivate: [AuthGuard] },]
                },
        ]
    },
    { path: 'admin', loadChildren: './admin/admin.module#AdminModule' },
    { path: 'login', component: LoginComponent },
    { path: '**', component: PageNotFoundComponent }
];

export const routing = RouterModule.forRoot(APP_ROUTES);
