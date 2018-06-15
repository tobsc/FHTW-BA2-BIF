import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {RouterModule, Router} from "@angular/router";
import { DashboardComponent } from './dashboard/dashboard.component';
import { DeviceListComponent } from './devices/device-list.component';
import { DevicesStatusDirective } from './devices/devices-status.directive';
import { CoreModule } from "../core/core.module";
import {AlertModule, AccordionModule, CollapseModule, PaginationModule} from "ngx-bootstrap";
import { CartComponent } from './cart/cart.component';
import { ModalModule } from 'ngx-modialog';
import { BootstrapModalModule } from 'ngx-modialog/plugins/bootstrap';
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
import { DashboardOrderListComponent } from './dashboard/dashboard-order-list/dashboard-order-list.component';
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {JwtErrorService} from "../shared/services/jwt-error.service";
import { Daterangepicker } from 'ng2-daterangepicker';
import { NguiAutoCompleteModule } from '@ngui/auto-complete';


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
        ModalModule.forRoot(),
        CollapseModule.forRoot(),
        BootstrapModalModule,
        ReactiveFormsModule,
        ConfirmDialogModule,
        PaginationModule.forRoot(),
        AlertModule.forRoot(),
        Daterangepicker,
        NguiAutoCompleteModule

    ],
    providers: [
      {
        provide: HTTP_INTERCEPTORS,
        useClass: JwtHttpService,
        multi: true
      },
      {
        provide: HTTP_INTERCEPTORS,
        useClass: JwtErrorService,
        multi: true,
      },
    ]
})
export class UserModule {}
