import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AdminDashboardComponent} from "./admin-dashboard/admin-dashboard.component";
import {adminRouting} from "./admin.routing";
import {DeviceListComponent} from "./devices/device-list/device-list.component";
import {CoreModule} from "../core/core.module";
import {DeviceTypesComponent} from "./devices/device-types/device-types.component";
import {DeviceTypesListComponent} from "./devices/device-types/device-types-list/device-types-list.component";
import { DeviceTypesAddComponent } from "./devices/device-types/device-types-add/device-types-add.component";
import { DeviceTypesEditComponent } from "./devices/device-types/device-types-edit/device-types-edit.component";
import {DeviceCustomFieldsComponent} from "./devices/device-custom-fields/device-custom-fields.component";
import {DeviceCustomFieldsFieldgroupsListComponent} from "./devices/device-custom-fields/device-custom-fields-fieldgroups-list/device-custom-fields-fieldgroups-list.component";
import { DeviceCustomFieldsFieldgroupsAddComponent } from "./devices/device-custom-fields/device-custom-fields-fieldgroups-add/device-custom-fields-fieldgroups-add.component";
import { DeviceCustomFieldsFieldgroupsEditComponent } from "./devices/device-custom-fields/device-custom-fields-fieldgroups-edit/device-custom-fields-fieldgroups-edit.component";
import {Ng2AutoCompleteModule} from "ng2-auto-complete";
import {FieldsToArrayPipe} from "./devices/device-add/fields-to-array.pipe";
import {ToArrayPipe} from "../shared/pipes/to-array.pipe";
import {XHRBackend, RequestOptions} from "@angular/http";
import {Router, RouterModule} from "@angular/router";
import {AuthService} from "../authentication/auth.service";
import {PubSubService} from "../shared/services/pub-sub.service";
import {JwtHttpService} from "../shared/services/jwt-http.service";
import {DeviceFormComponent} from "./devices/device-form/device-form.component";
import {DeviceEditComponent} from "./devices/device-edit/device-edit.component";
import {DeviceAddComponent} from "./devices/device-add/device-add.component";
import {AlertModule, CollapseModule, PaginationModule, TooltipModule} from "ng2-bootstrap";
import {DeviceDuplicateComponent} from "./devices/device-duplicate/device-duplicate.component";
import {ModalModule} from "angular2-modal";
import {ConfirmDialogModule} from "../core/confirm-dialog/confirm-dialog.module";
import { AdminSettingsComponent } from './admin-settings/admin-settings.component';
import { Daterangepicker } from 'ng2-daterangepicker';
import { AdminOrdersComponent } from './admin-orders/admin-orders.component';
import { AdminOrderListComponent } from './admin-orders/admin-order-list/admin-order-list.component';
import { AdminSingleOrderComponent } from './admin-orders/admin-single-order/admin-single-order.component';
import { AdminLogsComponent } from './admin-logs/admin-logs.component';
import { DeviceCustomFieldsFieldgroupsFormComponent } from './devices/device-custom-fields/device-custom-fields-fieldgroups-form/device-custom-fields-fieldgroups-form.component';
import { DamagesComponent } from './devices/damages/damages.component';
import { DamagesListComponent } from './devices/damages/damages-list/damages-list.component';
import { DamagesAddComponent } from './devices/damages/damages-add/damages-add.component';
import { DamageFormComponent } from './devices/damages/damage-form/damage-form.component';
import { DeviceTypesEditFormComponent } from './devices/device-types/device-types-edit-form/device-types-edit-form.component';
import { DeviceAccessoriesComponent } from './devices/device-accessories/device-accessories.component';
import {TextMaskModule} from "angular2-text-mask";
import { AddAdminComponent } from './admin-settings/edit-admins/add-admin/add-admin.component';
import { EditAdminsComponent } from './admin-settings/edit-admins/edit-admins.component';
import { RemoveAdminComponent } from './admin-settings/edit-admins/remove-admin/remove-admin.component';
import { DeviceAccessoryListComponent } from './devices/device-accessories/device-accessory-list/device-accessory-list.component';
import { DeviceAccessoryAddComponent } from './devices/device-accessories/device-accessory-add/device-accessory-add.component';
import { AdminMyOrderListComponent } from './admin-orders/admin-my-order-list/admin-my-order-list.component';

import { UiSwitchModule } from 'angular2-ui-switch/src'


export function jwtFactory(backend: XHRBackend, options: RequestOptions, router: Router, authService: AuthService, pubsub: PubSubService) {
    return new JwtHttpService(backend, options, router, authService, pubsub);
}


@NgModule({
    declarations: [
        AdminDashboardComponent,
        DeviceListComponent,
        DeviceAddComponent,
        DeviceTypesComponent,
        DeviceTypesListComponent,
        DeviceTypesAddComponent,
        DeviceTypesEditComponent,
        DeviceCustomFieldsComponent,
        DeviceCustomFieldsFieldgroupsListComponent,
        DeviceCustomFieldsFieldgroupsAddComponent,
        DeviceCustomFieldsFieldgroupsEditComponent,
        FieldsToArrayPipe,
        ToArrayPipe,
        DeviceFormComponent,
        DeviceEditComponent,
        DeviceDuplicateComponent,
        AdminSettingsComponent,
        AdminOrdersComponent,
        AdminOrderListComponent,
        AdminSingleOrderComponent,
        AdminLogsComponent,
        DeviceCustomFieldsFieldgroupsFormComponent,
        DamagesComponent,
        DamagesListComponent,
        DamagesAddComponent,
        DamageFormComponent,
        DeviceTypesEditFormComponent,
        DeviceAccessoriesComponent,
        AddAdminComponent,
        EditAdminsComponent,
        RemoveAdminComponent,
        DeviceAccessoryListComponent,
        DeviceAccessoryAddComponent,
        AdminMyOrderListComponent,

    ],
    imports: [
        RouterModule,
        CoreModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        Ng2AutoCompleteModule,
        adminRouting,
        AlertModule.forRoot(),
        ModalModule.forRoot(),
        CollapseModule.forRoot(),
        Daterangepicker,
        ConfirmDialogModule,
        PaginationModule.forRoot(),
        TooltipModule.forRoot(),
        TextMaskModule,
        UiSwitchModule
    ],
    providers: [
        {
            provide: JwtHttpService,
            useFactory: jwtFactory,
            deps: [XHRBackend, RequestOptions, Router, AuthService, PubSubService]
        },
    ],
})
export class AdminModule {
}