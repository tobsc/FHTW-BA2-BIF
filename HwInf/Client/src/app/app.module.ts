import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {HttpModule, XHRBackend, RequestOptions} from '@angular/http';

import { AppComponent } from './app.component';
import {JwtHttpService} from "./shared/jwt-http.service";
import {Router} from "@angular/router";

import { routing } from "./app.routing";
import { LoginComponent } from './authentication/login.component';
import { AuthService} from "./authentication/auth.service";
import { UserModule} from "./user/user.module";
import { AdminModule} from "./admin/admin.module";
import { DropdownModule, CollapseModule } from 'ng2-bootstrap';
import { AuthGuard } from "./authentication/auth.guard";
import {CoreModule} from "./core/core.module";



export function jwtFactory(backend: XHRBackend, options: RequestOptions, router: Router ) {
    return new JwtHttpService(backend, options, router);
}

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
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
        CollapseModule.forRoot(),
    ],
    providers: [
        {
            provide: JwtHttpService,
            useFactory: jwtFactory,
            deps: [XHRBackend, RequestOptions, Router ]
        },
        AuthService,
        AuthGuard,
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
