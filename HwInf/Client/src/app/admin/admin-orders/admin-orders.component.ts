import { Component, OnInit } from '@angular/core';
import {OrderService} from "../../shared/services/order.service";
import {OrderFilter} from "../../shared/models/order-filter.model";
import {OrderItem} from "../../shared/models/order-item.model";

@Component({
  selector: 'hwinf-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit {

  private openOrderFilter: OrderFilter;
  private activeOrderFilter: OrderFilter;
  private closedOrderFilter: OrderFilter;

  private openOrderItems: OrderItem[] = [];
  private activeOrderItems: OrderItem[] = [];
  private closedOrderItems: OrderItem[] = [];

  private showClosedOrders: boolean = false;

  constructor(
      private orderService: OrderService,
  ) { }

  ngOnInit() {
    this.openOrderFilter = new OrderFilter();
    this.openOrderFilter.StatusQuery = ['offen'];
    this.openOrderFilter.IsAdminView = true;

    this.activeOrderFilter = new OrderFilter();
    this.activeOrderFilter.StatusQuery = ['akzeptiert'];
    this.activeOrderFilter.IsAdminView = true;

    this.closedOrderFilter = new OrderFilter();
    this.closedOrderFilter.StatusQuery = ['abgeschlossen', 'abgelehnt'];
    this.closedOrderFilter.IsAdminView = true;

    this.fetchData();
  }

  fetchData() {
    this.orderService.getFilteredOrders(this.openOrderFilter).subscribe((data) => this.openOrderItems = data);
    this.orderService.getFilteredOrders(this.activeOrderFilter).subscribe((data) => this.activeOrderItems = data);
    this.orderService.getFilteredOrders(this.closedOrderFilter).subscribe((data) => this.closedOrderItems = data);
  }

  onToggleShowClosedOrders() {
    this.showClosedOrders = !this.showClosedOrders;
  }

  updateList() {
    this.fetchData();
  }

}
