import {NgModule} from "@angular/core";
import {CartComponent} from "./cart.component";
import {CommonModule} from "@angular/common";
import {cartRouting} from "./cart.routing";

@NgModule({
  declarations: [
    CartComponent
  ],
  imports: [
    CommonModule,
    cartRouting
  ]
})
export class CartModule {}
