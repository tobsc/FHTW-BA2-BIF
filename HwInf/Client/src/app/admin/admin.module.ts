import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import {adminRouting} from "./admin.routing";
import { DeviceListComponent } from './devices/device-list/device-list.component';
import {CoreModule} from "../core/core.module";
import { DeviceAddComponent } from './devices/device-add/device-add.component';
import { DeviceTypesComponent } from './devices/device-types/device-types.component';
import { DeviceTypesListComponent } from './devices/device-types/device-types-list/device-types-list.component';
import { DeviceTypesAddComponent } from './devices/device-types/device-types-add/device-types-add.component';
import { DeviceCustomFieldsComponent } from './devices/device-custom-fields/device-custom-fields.component';
import { DeviceCustomFieldsFieldgroupsListComponent } from './devices/device-custom-fields/device-custom-fields-fieldgroups-list/device-custom-fields-fieldgroups-list.component';
import { DeviceCustomFieldsFieldgroupsAddComponent } from './devices/device-custom-fields/device-custom-fields-fieldgroups-add/device-custom-fields-fieldgroups-add.component';

@NgModule({
    declarations: [
        AdminDashboardComponent,
        DeviceListComponent,
        DeviceAddComponent,
        DeviceTypesComponent,
        DeviceTypesListComponent,
        DeviceTypesAddComponent,
        DeviceCustomFieldsComponent,
        DeviceCustomFieldsFieldgroupsListComponent,
        DeviceCustomFieldsFieldgroupsAddComponent,
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