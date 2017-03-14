import { Routes, RouterModule } from "@angular/router";
import { PageNotFoundComponent } from "./core/page-not-found/page-not-found.component";
import { LoginComponent } from "./authentication/login.component";
import { DashboardComponent } from "./user/dashboard/dashboard.component";
import {   AuthGuard } from "./authentication/auth.guard";

const APP_ROUTES: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
    { path: '**', component: PageNotFoundComponent }
];

export const routing = RouterModule.forRoot(APP_ROUTES);
