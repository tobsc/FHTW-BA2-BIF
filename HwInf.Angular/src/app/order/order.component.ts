import { Component, OnInit } from '@angular/core';
import {CartService} from "../cart/cart.service";
import {Device} from "../devices/shared/device.model";
import {UserService} from "../shared/user.service";
import {User} from "../shared/user.model";
import {Observable} from "rxjs";

@Component({
  selector: 'hw-inf-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {

  private items: Device[] = [];
  private user: Observable<User>;
  constructor(private cartService: CartService, private userService: UserService) { }

  ngOnInit() {
    this.items = this.cartService.getItems();
    this.user = this.userService.getUser();
  }

}
