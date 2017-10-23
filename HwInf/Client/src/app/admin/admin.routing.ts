import {Routes, RouterModule} from "@angular/router";
import { AuthGuard } from "../authentication/auth.guard";
import {AdminDashboardComponent} from "./admin-dashboard/admin-dashboard.component";
import {HomeComponent} from "../home/home.component";
import {DeviceListComponent} from "../admin/devices/device-list/device-list.component";
import {DeviceAddComponent} from "./devices/device-add/device-add.component";
import { DeviceTypesComponent } from "./devices/device-types/device-types.component";
import { DeviceTypesEditComponent } from "./devices/device-types/device-types-edit/device-types-edit.component";
import { DeviceTypesAddComponent } from "./devices/device-types/device-types-add/device-types-add.component";
import { DeviceCustomFieldsComponent } from "./devices/device-custom-fields/device-custom-fields.component";
import { DeviceCustomFieldsFieldgroupsEditComponent } from "./devices/device-custom-fields/device-custom-fields-fieldgroups-edit/device-custom-fields-fieldgroups-edit.component";
import { DeviceCustomFieldsFieldgroupsAddComponent } from "./devices/device-custom-fields/device-custom-fields-fieldgroups-add/device-custom-fields-fieldgroups-add.component";
import { VerwalterGuard } from "../authentication/verwalter.guard";
import {DeviceEditComponent} from "./devices/device-edit/device-edit.component";
import {PageNotFoundComponent} from "../core/page-not-found/page-not-found.component";
import { DeviceDuplicateComponent } from "./devices/device-duplicate/device-duplicate.component";
import { AdminSettingsComponent } from "./admin-settings/admin-settings.component";
import {AdminOrdersComponent} from "./admin-orders/admin-orders.component";
import { AdminLogsComponent } from "./admin-logs/admin-logs.component";
import { DamagesComponent } from "./devices/damages/damages.component";
import { DamagesListComponent } from "./devices/damages/damages-list/damages-list.component";
import { DamagesAddComponent } from "./devices/damages/damages-add/damages-add.component";
import { DeviceAccessoriesComponent } from "./devices/device-accessories/device-accessories.component";
import { EditAdminsComponent } from "./admin-settings/edit-admins/edit-admins.component";
import { AddAdminComponent } from "./admin-settings/edit-admins/add-admin/add-admin.component";
import { AdminMyOrderListComponent } from "./admin-orders/admin-my-order-list/admin-my-order-list.component";





const ADMIN_ROUTES: Routes = [
    { path: 'admin', component: HomeComponent, canActivate: [AuthGuard , VerwalterGuard],
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full', },
            { path: 'dashboard', component: AdminDashboardComponent },
            { path: 'geraete/verwalten', component: DeviceListComponent },
            { path: 'geraete/verwalten/:invnum', component: DeviceEditComponent },
            { path: 'geraete/erstellen', component: DeviceAddComponent },
            { path: 'geraete/erstellen/:invnum', component: DeviceDuplicateComponent },
            { path: 'geraete/felder', component: DeviceCustomFieldsComponent },
            { path: 'geraete/typen', component: DeviceTypesComponent },
            { path: 'geraete/zubehoer', component: DeviceAccessoriesComponent },
            { path: 'schaden/geraet/:invnum', component: DamagesComponent },
            { path: 'schaden/verwalten', component: DamagesListComponent },
            { path: 'schaden/add', component: DamagesAddComponent},
            { path: 'settings', component: AdminSettingsComponent },
            { path: 'orders', redirectTo: 'orders/offen', pathMatch: 'full'},
            { path: 'orders/status/:status', component: AdminOrdersComponent },
            { path: 'orders/myorders', component: AdminMyOrderListComponent },
            { path: 'settings/logs', component: AdminLogsComponent },
            { path: 'settings/edit-admin', component: EditAdminsComponent },
            { path: 'settings/add-admin', component: AddAdminComponent },
            { path: '**', component: PageNotFoundComponent }
        ]
    }
];
export const adminRouting = RouterModule.forChild(ADMIN_ROUTES);
