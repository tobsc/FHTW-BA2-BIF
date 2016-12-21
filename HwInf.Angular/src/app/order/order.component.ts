import { Component, OnInit } from '@angular/core';
import {CartService} from "../cart/cart.service";
import {Device} from "../devices/shared/device.model";
import {UserService} from "../shared/user.service";
import {User} from "../shared/user.model";
import {Observable} from "rxjs";
import {NgForm} from "@angular/forms";
import {OrderService} from "./order.service";
import {Order} from "./order.model";

@Component({
  selector: 'hw-inf-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {

  private outgoingOrders: Observable<Order[]>;
  private incomingOrders: Observable<Order[]>;

  constructor(private cartService: CartService,
              private userService: UserService,
              private orderService: OrderService) { }


  ngOnInit(): void {
    this.fetchData();
  }

  private fetchData() {
    this.outgoingOrders = this.orderService.getOutgoingOrders();
    this.incomingOrders = this.orderService.getIncomingOrders();
  }

  public onAcceptOrder(id: number) {
    this.orderService.acceptOrder(id)
      .subscribe(
        (success) => {console.log(success); this.fetchData();},
        (error)   => {console.log(error)}
      );
  }
  public onDeclineOrder(id: number) {
    this.orderService.declineOrder(id)
      .subscribe(
        (success) => {console.log(success); this.fetchData();},
        (error)   => {console.log(error)}
      );
  }
}
