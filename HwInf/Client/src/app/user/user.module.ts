import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {RouterModule, Router} from "@angular/router";
import { DashboardComponent } from './dashboard/dashboard.component';
import { DeviceListComponent } from './devices/device-list.component';
import { DevicesStatusDirective } from './devices/devices-status.directive';
import { CoreModule } from "../core/core.module";
import {AccordionModule, CollapseModule} from "ng2-bootstrap";
import { CartComponent } from './cart/cart.component';
import { OrderStep1Component } from './cart/order-step1/order-step1.component';
import { OrderStep2Component } from './cart/order-step2/order-step2.component';
import { OrderComponent } from './cart/order.component';
import { OrderStep3Component } from './cart/order-step3/order-step3.component';
import { Daterangepicker } from 'ng2-daterangepicker';
import { ModalModule } from 'angular2-modal';
import { BootstrapModalModule } from 'angular2-modal/plugins/bootstrap';
import {JwtHttpService} from "../shared/services/jwt-http.service";
import {XHRBackend, RequestOptions} from "@angular/http";
import {AuthService} from "../authentication/auth.service";
import {PubSubService} from "../shared/services/pub-sub.service";
import { DeviceFilterComponent } from './devices/device-filter/device-filter.component';


export function jwtFactory(backend: XHRBackend, options: RequestOptions, router: Router, authService: AuthService, pubsub: PubSubService) {
    return new JwtHttpService(backend, options, router, authService, pubsub);
}

@NgModule({
    declarations: [
        DashboardComponent,
        DeviceListComponent,
        DevicesStatusDirective,
        CartComponent,
        OrderStep1Component,
        OrderStep2Component,
        OrderComponent,
        OrderStep3Component,
        DeviceFilterComponent
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
        ReactiveFormsModule
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