import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import { DashboardComponent } from './dashboard/dashboard.component';
import { DeviceListComponent } from './devices/device-list.component';
import { DeviceFilterComponent } from './devices/device-filter.component';
import { DevicesStatusDirective } from './devices/devices-status.directive';
@NgModule({
    declarations: [DashboardComponent, DeviceListComponent, DeviceFilterComponent, DevicesStatusDirective],
    imports: [CommonModule, FormsModule]
})
export class UserModule {}