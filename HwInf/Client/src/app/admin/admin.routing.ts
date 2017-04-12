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
import { VerwalterGuard } from "../authentication/verwalter.guard";
import {DeviceEditComponent} from "./devices/device-edit/device-edit.component";
import {PageNotFoundComponent} from "../core/page-not-found/page-not-found.component";
import {DeviceDuplicateComponent} from "./devices/device-duplicate/device-duplicate.component";

const ADMIN_ROUTES: Routes = [
    { path: 'admin', component: HomeComponent, canActivate: [AuthGuard , VerwalterGuard],
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full', },
            { path: 'dashboard', component: AdminDashboardComponent },
            { path: 'geraete', component: DeviceListComponent },
            { path: 'geraete/neu', component: DeviceAddComponent },
            { path: 'geraete/neu/:invnum', component: DeviceDuplicateComponent },
            { path: 'geraete/typen', component: DeviceTypesComponent,
                children: [{ path: '', redirectTo: 'verwalten', pathMatch: 'full' },
                    { path: 'edit/:slug', component: DeviceTypesEditComponent, canActivate: [AuthGuard] },
                    { path: 'verwalten', component: DeviceTypesAddComponent, canActivate: [AuthGuard] },]
            },
            { path: 'geraete/eigene_felder', component: DeviceCustomFieldsComponent },
            { path: 'geraete/page/:page', component: DeviceListComponent },
            { path: 'geraete/invnum/:invnum', component: DeviceEditComponent },
            { path: '**', component: PageNotFoundComponent }
        ]
    }
];
export const adminRouting = RouterModule.forChild(ADMIN_ROUTES);
