import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpModule, XHRBackend, RequestOptions } from '@angular/http';

import { AppComponent } from './app.component';
import {JwtHttpService} from "./shared/services/jwt-http.service";
import {Router} from "@angular/router";

import { routing } from "./app.routing";
import { LoginComponent } from './authentication/login.component';
import { AuthService } from "./authentication/auth.service";
import { CartService } from "./shared/services/cart.service";
import { UserModule} from "./user/user.module";
import { AdminModule} from "./admin/admin.module";
import { AlertModule, DropdownModule, CollapseModule, AccordionModule } from 'ng2-bootstrap';
import { ModalModule } from 'angular2-modal';
import { BootstrapModalModule } from 'angular2-modal/plugins/bootstrap';
import { AuthGuard } from "./authentication/auth.guard";
import { AdminGuard } from "./authentication/admin.guard";
import { VerwalterGuard } from "./authentication/verwalter.guard";
import { JwtService } from "./shared/services/jwt.service";
import { UserService } from "./shared/services/user.service";
import { ImpersonateService } from "./shared/services/impersonate.service";

import {CoreModule} from "./core/core.module";
import { HomeComponent } from './home/home.component';
import { DeviceService } from "./shared/services/device.service";
import { FeedbackHttpService } from "./shared/services/feedback-http.service";
import { PubSubService } from "./shared/services/pub-sub.service";
import { CustomFieldsService } from "./shared/services/custom-fields.service";
import { ErrorHandlerService } from "./shared/services/error-handler.service";
import { ErrorHandlerComponent } from "./shared/services/error-handler.component";
import { Daterangepicker } from 'ng2-daterangepicker';
import {KeysPipe} from "./shared/pipes/keys.pipe";
import {DeviceAddComponent} from "./admin/devices/device-add/device-add.component";
import {OrderService} from "./shared/services/order.service";
import { LoginAsComponent } from './authentication/login-as/login-as.component';




export function feedbackHttpFactory(backend: XHRBackend, options: RequestOptions, router: Router, pubsub: PubSubService) {
    return new FeedbackHttpService(backend, options, router, pubsub);
}
@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        HomeComponent,
        KeysPipe,
        ErrorHandlerComponent,
        LoginAsComponent,
    ],
    imports: [
        CoreModule,
        BrowserModule,
        FormsModule,
        HttpModule,
        routing,
        UserModule,
        AdminModule,
        DropdownModule.forRoot(),
        AccordionModule.forRoot(),
        AlertModule.forRoot(),
        CollapseModule.forRoot(),
        Daterangepicker,
        ModalModule.forRoot(),
        BootstrapModalModule
    ],
    providers: [
        {
            provide: FeedbackHttpService,
            useFactory: feedbackHttpFactory,
            deps: [XHRBackend, RequestOptions, Router, PubSubService]
        },
        AuthService,
        AuthGuard,
        AdminGuard,
        VerwalterGuard,
        DeviceService,
        JwtService,
        ImpersonateService,
        CartService,
        UserService,
        PubSubService,
        CustomFieldsService,
        ErrorHandlerService,
        OrderService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
