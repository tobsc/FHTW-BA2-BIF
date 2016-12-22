import {Component, OnInit, OnDestroy} from '@angular/core';
import {CartService} from "../cart/cart.service";
import {Device} from "../devices/shared/device.model";
import {UserService} from "../shared/user.service";
import {User} from "../shared/user.model";
import {Observable, Subscription} from "rxjs";
import {NgForm} from "@angular/forms";
import {OrderService} from "./order.service";
import {Order} from "./order.model";
import {_finally} from "rxjs/operator/finally";
import {DeviceService} from "../devices/shared/device.service";
import {IDictionary} from "../shared/dictionary.interface";
import {Dictionary} from "../shared/dictionary.class";

@Component({
  selector: 'hw-inf-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {


  private outgoingOrders: Observable<Order[]>;
  private incomingOrders: Observable<Order[]>;
  private devices: IDictionary<Device> = new Dictionary<Device>();
  private sub: Subscription;
  constructor(private cartService: CartService,
              private userService: UserService,
              private orderService: OrderService,
              private deviceService: DeviceService) { }


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
        (success) => {
          console.log(success);
          this.fetchData();
        },
        (error)   => {console.log(error)},
      );
  }
  public onDeclineOrder(id: number) {
    this.orderService.declineOrder(id)
      .subscribe(
        (success) => {
          console.log(success);
          this.fetchData();
        },
        (error)   => {console.log(error)}
      );
  }
}
