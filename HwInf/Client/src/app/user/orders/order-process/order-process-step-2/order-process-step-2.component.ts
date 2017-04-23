import {Component, OnInit, OnDestroy} from '@angular/core';
import {OrderProcessService} from "../shared/order-process.service";
import {Router} from "@angular/router";
import {OrderFormDataService} from "../shared/order-form-data.service";
import {Order} from "../../../../shared/models/order.model";
import {Observable} from "rxjs";
import {CartService} from "../../../../shared/services/cart.service";
import {Device} from "../../../../shared/models/device.model";
import {OrderService} from "../../../../shared/services/order.service";
import {OrderItem} from "../../../../shared/models/order-item.model";

@Component({
  selector: 'hwinf-order-process-step-2',
  templateUrl: './order-process-step-2.component.html',
  styleUrls: ['./order-process-step-2.component.scss']
})
export class OrderProcessStep2Component implements OnInit {

  private order: Order;
  private devices: Device[];
  constructor(
      private orderProcessService: OrderProcessService,
      private orderFormDataService: OrderFormDataService,
      private orderService: OrderService,
      private cartService: CartService,
      private router: Router
  ) { }

  ngOnInit() {
    this.order = this.orderFormDataService.getData();
    this.devices = this.cartService.getItems();

    this.order.OrderItems = this.devices.map(i => {
      return new OrderItem(null, null, i, null, null, null);
    })
  }

  onBack() {
    this.router.navigate(['/anfrage/schritt-1']);
  }

  onContinue() {

    let x = ({
      From: this.order.From,
      To: this.order.To,
      OrderItems: [] = this.devices.map(device => {
        return ({Device: device});
      }),
      OrderReason: this.order.OrderReason
    });

   this.orderService.createOrder(x).subscribe(
        () => { this.router.navigate(['/anfrage/schritt-3']); },
        (err) => {console.log(err)}
    );

  }
}
