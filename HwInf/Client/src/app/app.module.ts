import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {HttpModule, XHRBackend, RequestOptions} from '@angular/http';

import { AppComponent } from './app.component';
import {JwtHttpService} from "./shared/jwt-http.service";
import {Router} from "@angular/router";
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

import { routing } from "./app.routing";
import { LoginComponent } from './login/login.component';
import {AuthService} from "./login/auth.service";
import {UserModule} from "./user/user.module";
import {AdminModule} from "./admin/admin.module";
import {DropdownModule, CollapseModule} from 'ng2-bootstrap';
import { SidebarComponent } from './ui/sidebar.component';
import { TopNavbarComponent } from './ui/top-navbar.component';
import { FooterComponent } from './ui/footer.component';



export function jwtFactory(backend: XHRBackend, options: RequestOptions, router: Router ) {
    return new JwtHttpService(backend, options, router);
}

@NgModule({
    declarations: [
        AppComponent,
        PageNotFoundComponent,
        LoginComponent,
        SidebarComponent,
        TopNavbarComponent,
        FooterComponent,
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        routing,
        UserModule,
        AdminModule,
        DropdownModule.forRoot(),
        CollapseModule.forRoot()
    ],
    providers: [
        AuthService,
        {
            provide: JwtHttpService,
            useFactory: jwtFactory,
            deps: [XHRBackend, RequestOptions, Router ]
        },
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
