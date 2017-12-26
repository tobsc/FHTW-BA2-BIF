import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {RouterModule, Router} from "@angular/router";
import { DashboardComponent } from './dashboard/dashboard.component';
import { DeviceListComponent } from './devices/device-list.component';
import { DevicesStatusDirective } from './devices/devices-status.directive';
import { CoreModule } from "../core/core.module";
import {AccordionModule, CollapseModule, PaginationModule} from "ng2-bootstrap";
import { CartComponent } from './cart/cart.component';
import { Daterangepicker } from 'ng2-daterangepicker';
import { ModalModule } from 'angular2-modal';
import { BootstrapModalModule } from 'angular2-modal/plugins/bootstrap';
import {JwtHttpService} from "../shared/services/jwt-http.service";
import {XHRBackend, RequestOptions} from "@angular/http";
import {AuthService} from "../authentication/auth.service";
import {PubSubService} from "../shared/services/pub-sub.service";
import { DeviceFilterComponent } from './devices/device-filter/device-filter.component';
import {ConfirmDialogModule} from "../core/confirm-dialog/confirm-dialog.module";
import { OrderProcessComponent } from './orders/order-process/order-process.component';
import { OrderProcessStep1Component } from './orders/order-process/order-process-step-1/order-process-step-1.component';
import { OrderProcessStep2Component } from './orders/order-process/order-process-step-2/order-process-step-2.component';
import { OrderProcessStep3Component } from './orders/order-process/order-process-step-3/order-process-step-3.component';
import { MyOrdersComponent } from './orders/my-orders/my-orders.component';
import { OrdersArchivComponent } from './orders/orders-archiv/orders-archiv.component';
import { SingleOrderComponent } from './orders/single-order/single-order.component';
import { OrderConfirmComponent } from './orders/order-process/order-confirm/order-confirm.component';
import { AlertModule } from 'ng2-bootstrap';
import { DashboardOrderListComponent } from './dashboard/dashboard-order-list/dashboard-order-list.component';
import { Ng2AutoCompleteModule } from "ng2-auto-complete";



export function jwtFactory(backend: XHRBackend, options: RequestOptions, router: Router, authService: AuthService, pubsub: PubSubService) {
    return new JwtHttpService(backend, options, router, authService, pubsub);
}

@NgModule({
    declarations: [
        DashboardComponent,
        DeviceListComponent,
        DevicesStatusDirective,
        CartComponent,
        DeviceFilterComponent,
        OrderProcessComponent,
        OrderProcessStep1Component,
        OrderProcessStep2Component,
        OrderProcessStep3Component,
        MyOrdersComponent,
        OrdersArchivComponent,
        SingleOrderComponent,
        OrderConfirmComponent,
        DashboardOrderListComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        CoreModule,
        RouterModule,
        AccordionModule.forRoot(),
        Daterangepicker,
        ModalModule.forRoot(),
        CollapseModule.forRoot(),
        BootstrapModalModule,
        ReactiveFormsModule,
        ConfirmDialogModule,
        PaginationModule.forRoot(),
        AlertModule.forRoot(),
        Ng2AutoCompleteModule,
    ],
    providers: [
        {
            provide: JwtHttpService,
            useFactory: jwtFactory,
            deps: [XHRBackend, RequestOptions, Router, AuthService, PubSubService]
        },
    ]
})
export class UserModule {}