import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {HttpModule, XHRBackend, RequestOptions} from '@angular/http';

import { AppComponent } from './app.component';
import {JwtHttpService} from "./shared/services/jwt-http.service";
import {Router} from "@angular/router";

import { routing } from "./app.routing";
import { LoginComponent } from './authentication/login.component';
import { AuthService} from "./authentication/auth.service";
import { UserModule} from "./user/user.module";
import { AdminModule} from "./admin/admin.module";
import { AlertModule, DropdownModule, CollapseModule, AccordionModule} from 'ng2-bootstrap';
import { AuthGuard } from "./authentication/auth.guard";
import {CoreModule} from "./core/core.module";
import { HomeComponent } from './home/home.component';
import {DeviceService} from "./shared/services/device.service";



export function jwtFactory(backend: XHRBackend, options: RequestOptions, router: Router, authService: AuthService) {
    return new JwtHttpService(backend, options, router, authService);
}

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        HomeComponent,
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
    ],
    providers: [
        {
            provide: JwtHttpService,
            useFactory: jwtFactory,
            deps: [XHRBackend, RequestOptions, Router, AuthService]
        },
        AuthService,
        AuthGuard,
        DeviceService,
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
