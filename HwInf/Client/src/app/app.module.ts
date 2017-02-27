import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {HttpModule, XHRBackend, RequestOptions} from '@angular/http';

import { AppComponent } from './app.component';

import { DeviceService } from "./devices/shared/device.service";

import {AuthService} from "./authentication/auth.service";
import {AuthGuard} from "./authentication/auth.guard";
import {DevicesModule} from "./devices/devices.module";
import {SharedModule} from "./shared/shared.module";
import { ErrorMessageService } from "./shared/error-message/error-message.service";
import { DashboardModule } from "./dashboard/dashboard.module"
import {JwtHttpService} from "./shared/jwt-http.service";
import {Router} from "@angular/router";
import {CartModule} from "./cart/cart.module";
import {CartService} from "./cart/cart.service";
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import {AuthenticationModule} from "./authentication/authentication.module";
import {OrderModule} from "./order/order.module";
import {UserService} from "./shared/user.service";
import {OrderService} from "./order/order.service";
import { CoreModule } from "./core/core.module";
import { AdminModule } from "./admin/admin.module";
import { routing } from "./app.routing";



export function jwtFactory(backend: XHRBackend, options: RequestOptions, auth: AuthService, router: Router, errorMessageService: ErrorMessageService) {
    return new JwtHttpService(backend, options, auth, router, errorMessageService);
}

@NgModule({
    declarations: [
        AppComponent,
        PageNotFoundComponent,
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        DashboardModule,
        DevicesModule,
        SharedModule,
        CartModule,
        OrderModule,
        AuthenticationModule,
        CoreModule,
        AdminModule,
        routing,
    ],
    providers: [
      {
        provide: JwtHttpService,
        useFactory: jwtFactory,
        deps: [XHRBackend, RequestOptions, AuthService, Router, ErrorMessageService]
      },
      ErrorMessageService,
      DeviceService,
      AuthService,
      CartService,
      AuthGuard,
      UserService,
      OrderService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
