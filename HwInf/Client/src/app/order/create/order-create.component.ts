import { Component, OnInit } from '@angular/core';
import {Device} from "../../devices/shared/device.model";
import {Observable} from "rxjs";
import {User} from "../../shared/user.model";
import {CartService} from "../../cart/cart.service";
import {UserService} from "../../shared/user.service";
import {OrderService} from "../order.service";
import {NgForm} from "@angular/forms";
import {Order} from "../order.model";

@Component({
  selector: 'hw-inf-order-create',
  templateUrl: './order-create.component.html',
  styleUrls: ['./order-create.component.scss']
})
export class OrderCreateComponent implements OnInit {
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
    console.log(order);

    this.orderService.createOrder(order)
      .subscribe(
        (success) => {
          console.log("success");
          console.log(success);
          this.cartService.clear();
        },
        (error) => {
          console.log("error");
          console.log(error);
        }
      );
  }
}
