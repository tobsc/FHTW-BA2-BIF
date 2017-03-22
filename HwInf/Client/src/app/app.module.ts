import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpModule, XHRBackend, RequestOptions} from '@angular/http';

import { AppComponent } from './app.component';
import {JwtHttpService} from "./shared/services/jwt-http.service";
import {Router} from "@angular/router";

import { routing } from "./app.routing";
import { LoginComponent } from './authentication/login.component';
import { AuthService } from "./authentication/auth.service";
import { CartService } from "./shared/services/cart.service";
import { UserModule} from "./user/user.module";
import { AdminModule} from "./admin/admin.module";
import { AlertModule, DropdownModule, CollapseModule, AccordionModule} from 'ng2-bootstrap';
import { AuthGuard } from "./authentication/auth.guard";
import { AdminGuard } from "./authentication/admin.guard";
import { VerwalterGuard } from "./authentication/verwalter.guard";
import { JwtService } from "./shared/services/jwt.service";
import { UserService } from "./shared/services/user.service";

import {CoreModule} from "./core/core.module";
import { HomeComponent } from './home/home.component';
import { DeviceService } from "./shared/services/device.service";
import { FeedbackHttpService } from "./shared/services/feedback-http.service";
import { PubSubService } from "./shared/services/pub-sub.service";
import {CustomFieldsService} from "./shared/services/custom-fields.service";

import { Daterangepicker } from 'ng2-daterangepicker';
import {KeysPipe} from "./shared/pipes/keys.pipe";




export function jwtFactory(backend: XHRBackend, options: RequestOptions, router: Router, authService: AuthService, pubsub: PubSubService) {
    return new JwtHttpService(backend, options, router, authService, pubsub);
}

export function feedbackHttpFactory(backend: XHRBackend, options: RequestOptions, router: Router, pubsub: PubSubService) {
    return new FeedbackHttpService(backend, options, router, pubsub);
}
@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        HomeComponent,
        KeysPipe,

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
    ],
    providers: [
        {
            provide: JwtHttpService,
            useFactory: jwtFactory,
            deps: [XHRBackend, RequestOptions, Router, AuthService, PubSubService]
        },
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
        CartService,
        UserService,
        PubSubService,
        CustomFieldsService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
