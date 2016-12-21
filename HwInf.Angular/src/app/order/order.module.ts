import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import { OrderComponent } from './order.component';
import {orderRouting} from "./order.routing";
import {FormsModule} from "@angular/forms";
import {OrderService} from "./order.service";

@NgModule({
  declarations: [
    OrderComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    orderRouting
  ],
  providers: [
    OrderService
  ]
})
export class OrderModule {}
