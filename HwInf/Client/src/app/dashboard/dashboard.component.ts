import { Component, OnInit } from '@angular/core';
import {DeviceService} from "../devices/shared/device.service";
import {Device} from "../devices/shared/device.model";
import {Observable} from "rxjs";
import {OrderService} from "../order/order.service";
import {Order} from "../order/order.model";

@Component({
  selector: 'hw-inf-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  private test: Observable<Device[]>;
  private outgoingOrders: Observable<Order[]>;
  private incomingOrders: Observable<Order[]>;

  constructor(private deviceService: DeviceService,
              private orderService: OrderService) { }

  ngOnInit() {
    this.test = this.deviceService.getDevices('pc');
    this.outgoingOrders = this.orderService.getOutgoingOrders();
    this.incomingOrders = this.orderService.getIncomingOrders();
  }

}
