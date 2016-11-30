import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { NavComponent } from './nav.component';
import { DevicesComponent } from './devices/devices.component';
import { DeviceItemComponent } from './devices/device-list/device-item.component';
import { DeviceListComponent } from './devices/device-list/device-list.component';
import { DeviceService } from "./devices/device.service";
import { KeysPipe } from './pipes/keys.pipe';
import { DeviceDetailsComponent } from './devices/device-details/device-details.component';

@NgModule({
    declarations: [
        AppComponent,
        NavComponent,
        DevicesComponent,
        DeviceItemComponent,
        DeviceListComponent,
        KeysPipe,
        DeviceDetailsComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule
    ],
    providers: [DeviceService],
    bootstrap: [AppComponent]
})
export class AppModule { }
