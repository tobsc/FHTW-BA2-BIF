import {Routes, RouterModule} from "@angular/router";
import {AuthGuard} from "../authentication/auth.guard";
import {AdminDashboardComponent} from "./admin-dashboard/admin-dashboard.component";
import {HomeComponent} from "../home/home.component";
const ADMIN_ROUTES: Routes = [
    { path: 'admin', component: HomeComponent, canActivate: [AuthGuard],
        children: [
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
            { path: 'dashboard', component: AdminDashboardComponent, canActivate: [AuthGuard] },
        ]
    }
];
export const adminRouting = RouterModule.forChild(ADMIN_ROUTES);
