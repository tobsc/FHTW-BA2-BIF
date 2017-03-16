import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import {adminRouting} from "./admin.routing";
import { DeviceListComponent } from './devices/device-list/device-list.component';
import {CoreModule} from "../core/core.module";
import { DeviceAddComponent } from './devices/device-add/device-add.component';
@NgModule({
    declarations: [
        AdminDashboardComponent,
        DeviceListComponent,
        DeviceAddComponent
    ],
    imports: [
        CoreModule,
        CommonModule,
        FormsModule,
        adminRouting
    ]
})
export class AdminModule {}