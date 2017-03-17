import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import {adminRouting} from "./admin.routing";
import { DeviceListComponent } from './devices/device-list/device-list.component';
import {CoreModule} from "../core/core.module";
import { DeviceAddComponent } from './devices/device-add/device-add.component';
import { DeviceGroupsComponent } from './devices/device-groups/device-groups.component';
import { DeviceGroupsListComponent } from './devices/device-groups/device-groups-list/device-groups-list.component';
import { DeviceGroupsAddComponent } from './devices/device-groups/device-groups-add/device-groups-add.component';
import { DeviceTypesComponent } from './devices/device-types/device-types.component';
import { DeviceTypesListComponent } from './devices/device-types/device-types-list/device-types-list.component';
import { DeviceTypesAddComponent } from './devices/device-types/device-types-add/device-types-add.component';

@NgModule({
    declarations: [
        AdminDashboardComponent,
        DeviceListComponent,
        DeviceAddComponent,
        DeviceGroupsComponent,
        DeviceTypesComponent,
        DeviceTypesListComponent,
        DeviceTypesAddComponent,
        DeviceGroupsListComponent,
        DeviceGroupsAddComponent,

    ],
    imports: [
        CoreModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        adminRouting
    ],
})
export class AdminModule {}