import { Component, OnInit } from '@angular/core';
import {CartService} from "./cart.service";
import {Device} from "../devices/shared/device.model";

@Component({
  selector: 'hw-inf-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {

  private items: Device[];

  constructor(private cartService: CartService) { }

  ngOnInit() {
    this.items = this.cartService.getItems();
  }

  public removeItem(item: Device) {
    this.cartService.removeItem(item);
  }

}
