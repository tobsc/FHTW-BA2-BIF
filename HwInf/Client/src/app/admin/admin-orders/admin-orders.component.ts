import { Component, OnInit } from '@angular/core';
import {OrderService} from "../../shared/services/order.service";
import {OrderFilter} from "../../shared/models/order-filter.model";
import {OrderItem} from "../../shared/models/order-item.model";
import {Order} from "../../shared/models/order.model";

@Component({
  selector: 'hwinf-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit {

  private orders: Order[] = [];

  private undoStack: any = [];

  constructor(
      private orderService: OrderService,
  ) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData(): void {
    this.orderService.getOrders().subscribe(data => this.orders = data);
  }

  updateOrder(index: number, order: Order) {
    this.orders[index] = order;
  }


}
