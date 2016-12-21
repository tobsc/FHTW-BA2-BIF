import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import { OrderComponent } from './order.component';
import {orderRouting} from "./order.routing";
import {FormsModule} from "@angular/forms";
import {OrderService} from "./order.service";
import { OrderCreateComponent } from './create/order-create.component';

@NgModule({
  declarations: [
    OrderComponent,
    OrderCreateComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    orderRouting
  ],
  providers: [
  ]
})
export class OrderModule {}
