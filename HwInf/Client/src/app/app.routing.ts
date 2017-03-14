import { Routes, RouterModule } from "@angular/router";
import { PageNotFoundComponent } from "./page-not-found/page-not-found.component";
import { LoginComponent } from "./login/login.component";
import { DashboardComponent } from "./user/dashboard/dashboard.component";
import {   AuthGuard } from "./login/auth.guard";

const APP_ROUTES: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
    { path: '**', component: PageNotFoundComponent }
];

export const routing = RouterModule.forRoot(APP_ROUTES);
