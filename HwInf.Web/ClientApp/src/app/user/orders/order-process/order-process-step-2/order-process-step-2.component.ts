import {Component, OnInit, OnDestroy} from '@angular/core';
import {Router} from "@angular/router";
import {OrderFormDataService} from "../shared/order-form-data.service";
import {Order} from "../../../../shared/models/order.model";
import {CartService} from "../../../../shared/services/cart.service";
import {Device} from "../../../../shared/models/device.model";
import { OrderItem } from "../../../../shared/models/order-item.model";
import { Modal } from 'ngx-modialog/plugins/bootstrap';
import {Accessory} from "../../../../shared/models/accessory.model";
import {DeviceService} from "../../../../shared/services/device.service";
import {Observable} from "rxjs/Observable";
import {Filter} from "../../../../shared/models/filter.model";

@Component({
  selector: 'hwinf-order-process-step-2',
  templateUrl: './order-process-step-2.component.html',
  styleUrls: ['./order-process-step-2.component.scss']
})
export class OrderProcessStep2Component implements OnInit, OnDestroy {

  public order: Order;
  public devices: Device[];
  public devicesWithoutInvNum: Device[];
  public filter: Filter;
  public accessories: Observable<Accessory[]>;
  public cartQuantity: number = 1;
  constructor(
      public orderFormDataService: OrderFormDataService,
      public cartService: CartService,
      public deviceService: DeviceService,
      public router: Router,
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
    this.accessories = this.deviceService.getAccessories();

    this.filter = new Filter();
    this.filter.DeviceType = '';
    this.filter.Order = 'ASC';
    this.filter.OrderBy = 'name';
    this.filter.Limit = 500;
    this.filter.Offset = 0;

    this.deviceService.getFilteredDevicesUser(this.filter).subscribe(
        (data) => {
          this.devicesWithoutInvNum = data.Devices.filter(x => x.Status.Description == "Verfügbar" && (x.InvNum == '' || x.InvNum == null));
          let groupSlugsToExclude = this.devices.map(x => x.DeviceGroupSlug);
          this.devicesWithoutInvNum = this.devicesWithoutInvNum.filter(d => groupSlugsToExclude.indexOf(d.DeviceGroupSlug) === -1);
        }
    );

  }

  onChange(ev, index: number) {
    if ( ev.target.checked ) {
      let newOrderItem = new OrderItem();
      console.log(ev.target.value);
      newOrderItem.Device = this.devicesWithoutInvNum.find(i => i.DeviceId == ev.target.value);
      if(newOrderItem.Device.Quantity < 1) {
        newOrderItem.Device.Quantity = 1;
      }
      newOrderItem.Accessories = [];
      this.order.OrderItems.push(newOrderItem);
      //this.order.OrderItems[index].Accessories.push(ev.target.value);
      console.log(this.order.OrderItems);
    }
    else {
      console.log("rem:" + ev.target.value);
      this.order.OrderItems = this.order.OrderItems.filter(i => i.Device.DeviceId != ev.target.value);
      //this.order.OrderItems[index].Accessories = this.order.OrderItems[index].Accessories.filter(i => i != ev.target.value);
      console.log(this.order.OrderItems);
    }
  }

  updateQuantity(ev, id) {
    if(this.order.OrderItems.find(i => i.Device.DeviceId == id)) {
      this.order.OrderItems.find(i => i.Device.DeviceId == id).Device.Quantity = ev.target.value;
    } else {
      this.devicesWithoutInvNum.find(i => i.DeviceId == id).Quantity = ev.target.value;
    }
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
