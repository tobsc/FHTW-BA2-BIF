import {Routes} from "@angular/router";
import {DeviceListComponent} from "./device-list/device-list.component";
import {DeviceDetailsComponent} from "./device-details/device-details.component";
export const DEVICES_ROUTES: Routes = [
    { path: '', component: DeviceListComponent },
    { path: ':id', component: DeviceDetailsComponent },
];
