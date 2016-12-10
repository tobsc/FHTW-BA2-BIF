import {Routes} from "@angular/router";
import {DeviceListComponent} from "./device-list/device-list.component";
import {DeviceDetailsComponent} from "./device-details/device-details.component";
import {DeviceAddComponent} from "./device-add/device-add.component";
import {DevicesStartComponent} from "./devices-start.component";
export const DEVICES_ROUTES: Routes = [
    { path: '', component: DevicesStartComponent },
    { path: 'id/:id', component: DeviceDetailsComponent },
    { path: 'type/:type', component: DeviceListComponent },
    { path: 'add', component: DeviceAddComponent },
];
