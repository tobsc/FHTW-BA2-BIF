import { BrowserModule } from '@angular/platform-browser';
import {NgModule, LOCALE_ID} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import {RouterModule, Router} from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ModalModule } from 'ngx-modialog';
import { BootstrapModalModule } from 'ngx-modialog/plugins/bootstrap';
import { AlertModule, AccordionModule, BsDropdownModule,CollapseModule } from 'ngx-bootstrap';
import { NguiAutoCompleteModule } from '@ngui/auto-complete';

import {CoreModule} from "./core/core.module";
import {SessionStorageService} from "./shared/services/session-storage.service";
import {PubSubSearchService} from "./shared/services/pub-sub-search.service";
import {AuthService} from "./authentication/auth.service";
import {AuthGuard} from "./authentication/auth.guard";
import {AdminGuard} from "./authentication/admin.guard";
import {VerwalterGuard} from "./authentication/verwalter.guard";
import {DeviceService} from "./shared/services/device.service";
import {JwtService} from "./shared/services/jwt.service";
import {AdminService} from "./shared/services/admin.service";
import {CartService} from "./shared/services/cart.service";
import {UserService} from "./shared/services/user.service";
import {PubSubService} from "./shared/services/pub-sub.service";
import {CustomFieldsService} from "./shared/services/custom-fields.service";
import {ErrorHandlerService} from "./shared/services/error-handler.service";
import {OrderService} from "./shared/services/order.service";
import {DamageService} from "./shared/services/damage.service";
import {ErrorHandlerComponent} from "./shared/services/error-handler.component";
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {JwtHttpService} from "./shared/services/jwt-http.service";
import {JwtErrorService} from "./shared/services/jwt-error.service";
import {FeedbackHttpService} from "./shared/services/feedback-http.service";
import {XHRBackend, RequestOptions, HttpModule} from "@angular/http";
import {routing} from "./app.routing";
import {UserModule} from "./user/user.module";
import {AdminModule} from "./admin/admin.module";
import { registerLocaleData } from '@angular/common';
import localeDEAT from '@angular/common/locales/de-AT';
import {LoginAsComponent} from "./authentication/login-as/login-as.component";
import {LoginComponent} from "./authentication/login.component";
import { Daterangepicker } from 'ng2-daterangepicker';
import {KeysPipe} from "./shared/pipes/keys.pipe"

export function feedbackHttpFactory(backend: XHRBackend, options: RequestOptions, router: Router, pubsub: PubSubService) {
  return new FeedbackHttpService(backend, options, router, pubsub);
}

registerLocaleData(localeDEAT);

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ErrorHandlerComponent,
    CounterComponent,
    FetchDataComponent,
    LoginAsComponent,
    LoginComponent,
    KeysPipe
  ],
  imports: [
    CoreModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    CoreModule,
    BrowserModule,
    FormsModule,
    CoreModule,
    BrowserModule,
    FormsModule,
    HttpModule,
    routing,
    UserModule,
    AdminModule,
    BsDropdownModule.forRoot(),
    AccordionModule.forRoot(),
    AlertModule.forRoot(),
    CollapseModule.forRoot(),
    Daterangepicker,
    ModalModule.forRoot(),
    BootstrapModalModule,
    NguiAutoCompleteModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
    ]),
  ],
  providers: [
    {
      provide: FeedbackHttpService,
      useFactory: feedbackHttpFactory,
      deps: [XHRBackend, RequestOptions, Router, PubSubService]
    },
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
    { provide: LOCALE_ID, useValue: "de-AT" },
    AuthService,
    AuthGuard,
    AdminGuard,
    VerwalterGuard,
    DeviceService,
    JwtService,
    AdminService,
    CartService,
    UserService,
    PubSubService,
    CustomFieldsService,
    ErrorHandlerService,
    OrderService,
    DamageService,
    PubSubSearchService,
    SessionStorageService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
