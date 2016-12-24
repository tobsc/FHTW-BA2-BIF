import {NgModule} from "@angular/core";
import {DeviceListComponent} from "./device-list/device-list.component";
import {DeviceItemComponent} from "./device-list/device-item.component";
import {DevicesComponent} from "./devices.component";
import {DeviceDetailsComponent} from "./device-details/device-details.component";
import {DevicesStartComponent} from "./devices-start.component";
import {DeviceFilterComponent} from "./device-list/device-filter.component";
import {DeviceStatusDirective} from "./shared/device-status.directive";
import {DeviceAddComponent} from "./device-add/device-add.component";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {devicesRouting} from "./devices.routing";
import {KeysPipe} from "../shared/pipes/keys.pipe";
import {OrderDeviceByPipe} from "./shared/order-device-by.pipe";
import {SharedModule} from "../shared/shared.module";
import {NavComponent} from "../core/nav/nav.component";
import {Router} from "@angular/router";
import {AuthService} from "../authentication/auth.service";
import {RequestOptions, XHRBackend} from "@angular/http";
import {JwtHttpService} from "../shared/jwt-http.service";
@NgModule({
  declarations: [
    DevicesComponent,
    DeviceItemComponent,
    DeviceListComponent,
    DeviceDetailsComponent,
    DeviceFilterComponent,
    DevicesStartComponent,
    DeviceStatusDirective,
    DeviceAddComponent,
    KeysPipe,
    OrderDeviceByPipe,
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    devicesRouting
  ],
})
export class DevicesModule { }
