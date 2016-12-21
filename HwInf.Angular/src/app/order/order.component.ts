import { Component, OnInit } from '@angular/core';
import {CartService} from "../cart/cart.service";
import {Device} from "../devices/shared/device.model";
import {Observable} from "rxjs";

@Component({
  selector: 'hw-inf-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {

  private items: Device[] = [];

  constructor(private cartService: CartService) { }

  ngOnInit() {
    this.items = this.cartService.getItems();
  }

}
