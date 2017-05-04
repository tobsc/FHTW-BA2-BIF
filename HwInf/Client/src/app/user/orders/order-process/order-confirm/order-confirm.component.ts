import { Component, OnInit } from '@angular/core';
import {CartService} from "../../../../shared/services/cart.service";

@Component({
  selector: 'hwinf-order-confirm',
  templateUrl: './order-confirm.component.html',
  styleUrls: ['./order-confirm.component.scss']
})
export class OrderConfirmComponent implements OnInit {

  constructor(
      private cartService: CartService
  ) { }

  ngOnInit() {
    this.cartService.clear();
  }

}
