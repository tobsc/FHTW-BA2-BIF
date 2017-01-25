import {Routes, RouterModule} from "@angular/router";
import {AuthGuard} from "../authentication/auth.guard";
import {AdminStartComponent} from "./admin-start.component";
import { TypesManagerComponent } from "./devices/types-manager/types-manager.component";
import { TypeAddComponent } from "./devices/types-manager/type-add/type-add.component";

const ADMIN_ROUTES: Routes = [
        { path: '', component: AdminStartComponent, canActivate:[AuthGuard]},
        { path: 'types', component: TypesManagerComponent, canActivate: [AuthGuard] },
        { path: 'types/new', component: TypeAddComponent, canActivate: [AuthGuard] },

];
export const adminRouting = RouterModule.forChild(ADMIN_ROUTES);
