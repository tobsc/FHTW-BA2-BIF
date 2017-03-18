import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { DashboardComponent } from './dashboard/dashboard.component';
import { DeviceListComponent } from './devices/device-list.component';
import { DevicesStatusDirective } from './devices/devices-status.directive';
import { CoreModule } from "../core/core.module";
import { AccordionModule } from "ng2-bootstrap";
import { CartComponent } from './cart/cart.component';
import { OrderStep1Component } from './cart/order-step1/order-step1.component';
import { OrderStep2Component } from './cart/order-step2/order-step2.component';
import { OrderComponent } from './cart/order.component';


@NgModule({
    declarations: [
        DashboardComponent,
        DeviceListComponent,
        DevicesStatusDirective,
        CartComponent,
        OrderStep1Component,
        OrderStep2Component,
        OrderComponent],
    imports: [
        CommonModule,
        FormsModule,
        CoreModule,
        RouterModule,
        AccordionModule.forRoot()
    ]
})
export class UserModule {}