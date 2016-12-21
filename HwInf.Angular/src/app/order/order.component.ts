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

  private items: Device[] = [];
  private user: Observable<User>;
  constructor(private cartService: CartService,
              private userService: UserService,
              private orderService: OrderService) { }

  ngOnInit() {
    this.items = this.cartService.getItems();
    this.user = this.userService.getUser();
  }

  private getOrderItems(): number[] {
    let arr: number[] = [];
    for (let device of this.items) {
      arr.push(device.DeviceId);
    }
    return arr;
  }

  public onSubmit(form: NgForm) {

    let order: Order = form.form.value;
    order.OrderItems = this.getOrderItems();

    this.orderService.createOrder(order).subscribe(
      (data) => {
        console.log("success");
        console.log(data);
      },
      (error) => {
        console.log("error");
        console.log(error);
      }
    );
  }

}
