import {Routes, RouterModule} from "@angular/router";
import {DeviceListComponent} from "./device-list/device-list.component";
import {DeviceDetailsComponent} from "./device-details/device-details.component";
import {DeviceAddComponent} from "./device-add/device-add.component";
import {DevicesStartComponent} from "./devices-start.component";
import {DevicesComponent} from "./devices.component";
import {AuthGuard} from "../shared/auth.guard";

const DEVICES_ROUTES: Routes = [
    { path: 'devices', component: DevicesComponent, canActivate: [AuthGuard], children: [
      { path: '', component: DevicesStartComponent },
      { path: 'id/:id', component: DeviceDetailsComponent },
      { path: 'type/:type', component: DeviceListComponent },
      { path: 'add', component: DeviceAddComponent },
    ]}

];
export const devicesRouting = RouterModule.forChild(DEVICES_ROUTES);
