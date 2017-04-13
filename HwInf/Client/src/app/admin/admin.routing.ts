import {Routes, RouterModule} from "@angular/router";
import { AuthGuard } from "../authentication/auth.guard";
import {AdminDashboardComponent} from "./admin-dashboard/admin-dashboard.component";
import {HomeComponent} from "../home/home.component";
import {DeviceListComponent} from "../admin/devices/device-list/device-list.component";
import {DeviceAddComponent} from "./devices/device-add/device-add.component";
import { DeviceTypesComponent } from "./devices/device-types/device-types.component";
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
            { path: 'geraete/verwalten', component: DeviceListComponent },
            { path: 'geraete/erstellen', component: DeviceAddComponent },
            { path: 'geraete/erstellen/:invnum', component: DeviceDuplicateComponent },
            { path: 'geraete/typen', component: DeviceTypesComponent },
            { path: 'geraete/felder', component: DeviceCustomFieldsComponent },
            { path: 'geraete/invnum/:invnum', component: DeviceEditComponent },
            { path: '**', component: PageNotFoundComponent }
        ]
    }
];
export const adminRouting = RouterModule.forChild(ADMIN_ROUTES);
