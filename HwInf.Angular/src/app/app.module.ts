import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {HttpModule, XHRBackend, RequestOptions} from '@angular/http';

import { AppComponent } from './app.component';

import { DeviceService } from "./devices/shared/device.service";

import {routing} from "./app.routing";
import { DashboardComponent } from './dashboard/dashboard.component';

import { LoginComponent } from './login/login.component';
import {AuthService} from "./shared/auth.service";
import {AuthGuard} from "./shared/auth.guard";
import {DevicesModule} from "./devices/devices.module";
import {SharedModule} from "./shared/shared.module";
import {ErrorMessageService} from "./shared/error-message/error-message.service";
import {JwtHttpService} from "./shared/jwt-http.service";
import {Router} from "@angular/router";

@NgModule({
    declarations: [
        AppComponent,
        DashboardComponent,
        LoginComponent,
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        routing,
        DevicesModule,
        SharedModule
    ],
    providers: [
      ErrorMessageService,
      DeviceService,
      AuthService,
      AuthGuard],
    bootstrap: [AppComponent]
})
export class AppModule { }
