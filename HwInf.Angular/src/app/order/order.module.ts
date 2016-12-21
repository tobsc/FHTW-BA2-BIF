import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import { OrderComponent } from './order.component';
import {orderRouting} from "./order.routing";

@NgModule({
  declarations: [
    OrderComponent
  ],
  imports: [
    CommonModule,
    orderRouting
  ]
})
export class OrderModule {}
