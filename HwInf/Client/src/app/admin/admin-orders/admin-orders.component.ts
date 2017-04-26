import { Component, OnInit } from '@angular/core';
import {OrderService} from "../../shared/services/order.service";
import {OrderFilter} from "../../shared/models/order-filter.model";
import {OrderItem} from "../../shared/models/order-item.model";
import {Order} from "../../shared/models/order.model";
import {OrderList} from "../../shared/models/order-list.model";

@Component({
  selector: 'hwinf-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit {

  private filter: OrderFilter = new OrderFilter();

  constructor(
      private orderService: OrderService,
  ) { }

  ngOnInit() {
  }
}
