import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { NavComponent } from './nav.component';

import { DeviceService } from "./devices/shared/device.service";

import {routing} from "./app.routing";
import { DashboardComponent } from './dashboard/dashboard.component';

import { SortPipe } from './pipes/sort.pipe';
import { PanelCollapseDirective } from './shared/panel-collapse.directive';

import { LoginComponent } from './login/login.component';
import {AuthService} from "./shared/auth.service";
import {AuthGuard} from "./shared/auth.guard";
import {DevicesModule} from "./devices/devices.module";
import {SharedModule} from "./shared/shared.module";

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
    providers: [DeviceService, AuthService, AuthGuard],
    bootstrap: [AppComponent]
})
export class AppModule { }
