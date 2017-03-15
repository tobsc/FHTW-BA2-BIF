import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import { DashboardComponent } from './dashboard/dashboard.component';
import { DeviceListComponent } from './devices/device-list.component';
@NgModule({
    declarations: [DashboardComponent, DeviceListComponent],
    imports: [CommonModule, FormsModule]
})
export class UserModule {}