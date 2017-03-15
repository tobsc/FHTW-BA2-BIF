import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import {adminRouting} from "./admin.routing";
@NgModule({
    declarations: [

    AdminDashboardComponent],
    imports: [
        CommonModule,
        FormsModule,
        adminRouting
    ]
})
export class AdminModule {}