import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { NavComponent } from './nav.component';
import { DevicesComponent } from './devices/devices.component';
import { DeviceItemComponent } from './devices/device-list/device-item.component';
import { DeviceListComponent } from './devices/device-list/device-list.component';
import { DeviceService } from "./devices/shared/device.service";
import { KeysPipe } from './pipes/keys.pipe';
import { DeviceDetailsComponent } from './devices/device-details/device-details.component';
import {routing} from "./app.routing";
import { DashboardComponent } from './dashboard/dashboard.component';
import { DeviceFilterComponent } from './devices/device-list/device-filter.component';
import { DevicesStartComponent } from './devices/devices-start.component';
import { OrderDeviceByPipe } from './devices/shared/order-device-by.pipe';
import { DeviceAddComponent } from './devices/device-add/device-add.component';
import { SortPipe } from './pipes/sort.pipe';
import { PanelCollapseDirective } from './shared/panel-collapse.directive';
import { ModalComponent } from './shared/modal/modal.component';

@NgModule({
    declarations: [
        AppComponent,
        NavComponent,
        DevicesComponent,
        DeviceItemComponent,
        DeviceListComponent,
        KeysPipe,
        DeviceDetailsComponent,
        DashboardComponent,
        DeviceFilterComponent,
        DevicesStartComponent,
        OrderDeviceByPipe,
        DeviceAddComponent,
        SortPipe,
        PanelCollapseDirective,
        ModalComponent,
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        routing
    ],
    providers: [DeviceService],
    bootstrap: [AppComponent]
})
export class AppModule { }
