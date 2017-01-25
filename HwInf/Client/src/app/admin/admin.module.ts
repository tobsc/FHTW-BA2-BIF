import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {SharedModule} from "../shared/shared.module";

import { AdminStartComponent } from './admin-start.component';
import {adminRouting} from "./admin.routing";
import { TypesManagerComponent } from './devices/types-manager/types-manager.component';
import { TypeAddComponent } from './devices/types-manager/type-add/type-add.component';
@NgModule({
  declarations: [
    AdminStartComponent,
    TypesManagerComponent,
    TypeAddComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    adminRouting
  ],
})
export class AdminModule { }
