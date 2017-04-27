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
import { OrderStep3Component } from "./user/cart/order-step3/order-step3.component";
import { OrderComponent } from "./user/cart/order.component";
import {OrderListComponent} from "./user/orders/order-list/order-list.component";
import {OrderProcessComponent} from "./user/orders/order-process/order-process.component";
import {OrderProcessStep1Component} from "./user/orders/order-process/order-process-step-1/order-process-step-1.component";
import {OrderProcessStep2Component} from "./user/orders/order-process/order-process-step-2/order-process-step-2.component";
import {OrderProcessStep3Component} from "./user/orders/order-process/order-process-step-3/order-process-step-3.component";
import {MyOrdersComponent} from "./user/orders/my-orders/my-orders.component";
import { LoginAsComponent } from "./authentication/login-as/login-as.component";

const APP_ROUTES: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard],
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
            { path: 'geraete', component: DeviceListComponent, canActivate: [AuthGuard] },
            { path: 'geraete/typ/:type', component: DeviceListComponent, canActivate: [AuthGuard] },
            { path: 'warenkorb', component: CartComponent, canActivate: [AuthGuard], },
            { path: 'anfrage', component: OrderProcessComponent, canActivate: [AuthGuard],
                children: [{ path: '', redirectTo: 'schritt-1', pathMatch: 'full' },
                            { path: 'schritt-1', component: OrderProcessStep1Component, canActivate: [AuthGuard] },
                            { path: 'schritt-2', component: OrderProcessStep2Component, canActivate: [AuthGuard] },
                            { path: 'schritt-3', component: OrderProcessStep3Component, canActivate: [AuthGuard] },]
                },
            { path: 'login-as', component: LoginAsComponent, canActivate: [AuthGuard]},
            { path: 'meine-geraete', component: MyOrdersComponent}
        ]
    },
    { path: 'admin', loadChildren: './admin/admin.module#AdminModule' },
    { path: 'login', component: LoginComponent },
    { path: '**', component: PageNotFoundComponent }
];

export const routing = RouterModule.forRoot(APP_ROUTES);
