import {Component, OnInit, OnDestroy} from '@angular/core';
import {Router} from "@angular/router";
import {OrderFormDataService} from "../shared/order-form-data.service";
import {Order} from "../../../../shared/models/order.model";
import {CartService} from "../../../../shared/services/cart.service";
import {Device} from "../../../../shared/models/device.model";
import { OrderItem } from "../../../../shared/models/order-item.model";
import { Modal } from 'angular2-modal/plugins/bootstrap';

@Component({
  selector: 'hwinf-order-process-step-2',
  templateUrl: './order-process-step-2.component.html',
  styleUrls: ['./order-process-step-2.component.scss']
})
export class OrderProcessStep2Component implements OnInit, OnDestroy {

  private order: Order;
  private devices: Device[];
  constructor(
      private orderFormDataService: OrderFormDataService,
      private cartService: CartService,
      private router: Router,
      public modal: Modal
  ) { }

  ngOnInit() {
    this.order = this.orderFormDataService.getData();
    this.devices = this.cartService.getItems();
    this.order.OrderItems = this.devices.map(i => {
      let orderItem = new OrderItem();
      orderItem.Device = i;
      orderItem.Accessories = [];
      return orderItem;
    });
    console.log( this.order);
  }

  onChange(ev, index: number) {
    if ( ev.target.checked ) {
      this.order.OrderItems[index].Accessories.push(ev.target.value);
    }
    else {
      this.order.OrderItems[index].Accessories = this.order.OrderItems[index].Accessories.filter(i => i != ev.target.value);
    }
  }

  getAccessoriesOfOrderItem(item: OrderItem) {
    return item.Device.DeviceMeta.filter(i => i.FieldGroupSlug === 'zubehor').map(i => i.Field);
  }

  onBack() {
    this.router.navigate(['/anfrage/schritt-1']);
  }

  onContinue() {
    this.router.navigate(['/anfrage/schritt-3']);
  }

  ngOnDestroy(): void {
    this.orderFormDataService.setData(this.order);
  }

}
