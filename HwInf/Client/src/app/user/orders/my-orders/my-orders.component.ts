import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {OrderService} from "../../../shared/services/order.service";
import {OrderItem} from "../../../shared/models/order-item.model";
import {OrderFilter} from "../../../shared/models/order-filter.model";

@Component({
  selector: 'hwinf-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.scss']
})
export class MyOrdersComponent implements OnInit {

  private openOrderFilter: OrderFilter;
  private activeOrderFilter: OrderFilter;
  private closedOrderFilter: OrderFilter;

  private openOrderItems: OrderItem[] = [];
  private activeOrderItems: OrderItem[] = [];
  private closedOrderItems: OrderItem[] = [];

  private showClosedOrders: boolean = false;

  constructor(
      private orderService: OrderService,
      private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.openOrderFilter = new OrderFilter();
    this.openOrderFilter.StatusQuery = ['offen'];

    this.activeOrderFilter = new OrderFilter();
    this.activeOrderFilter.StatusQuery = ['akzeptiert'];

    this.closedOrderFilter = new OrderFilter();
    this.closedOrderFilter.StatusQuery = ['abgeschlossen', 'abgelehnt'];

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

}
