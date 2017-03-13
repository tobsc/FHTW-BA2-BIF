import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import { DashboardComponent } from './dashboard/dashboard.component';
@NgModule({
    declarations: [DashboardComponent],
    imports: [CommonModule, FormsModule]
})
export class UserModule {}