import { Component, OnInit } from '@angular/core';
import {OrderProcessService} from "../shared/order-process.service";
import {CartService} from "../../../../shared/services/cart.service";

@Component({
  selector: 'hwinf-order-process-step-3',
  templateUrl: './order-process-step-3.component.html',
  styleUrls: ['./order-process-step-3.component.scss']
})
export class OrderProcessStep3Component implements OnInit {

  constructor(
      private orderProcessService: OrderProcessService,
      private cartService: CartService
  ) { }

  ngOnInit() {
    this.orderProcessService.setStatus(2, 'done');
    this.cartService.clear();
  }

}
