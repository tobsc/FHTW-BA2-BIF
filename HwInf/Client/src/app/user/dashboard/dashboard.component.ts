import { Component, OnInit } from '@angular/core';
import {DeviceService} from "../../shared/services/device.service";
import {Device} from "../../shared/models/device.model";
import {OrderService} from "../../shared/services/order.service";
import {Order} from "../../shared/models/order.model";
import {OrderFilter} from "../../shared/models/order-filter.model";
import {OrderItem} from "../../shared/models/order-item.model";

@Component({
  selector: 'hwinf-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  private myOrders: Order[];
  private totalItems: number;
  private filter: OrderFilter;

  private orderOpen: number;
  private orderAccept: number;
  private orderLend: number;

  constructor(private orderService: OrderService) { }

  ngOnInit(): void {

    this.filter = new OrderFilter();
    this.filter.StatusSlugs = ['offen', 'akzeptiert', 'ausgeliehen'];
    this.filter.Order = 'DESC';
    this.filter.OrderBy = 'orderstatus';
    this.filter.Limit = 100;
    this.filter.Offset = 0;

    this.fetchData();
  }

  fetchData() {

    this.orderService.getFilteredOrders(this.filter)
        .subscribe(
            data => {
              this.myOrders = data.Orders;
              this.totalItems = data.TotalItems;
              this.orderOpen = data.Orders.filter(i => i.OrderStatus.StatusId === 1).length;
              this.orderAccept = data.Orders.filter(i => i.OrderStatus.StatusId === 2).length;
              let devs = data.Orders.map(i => i.OrderItems);
              this.orderLend = [].concat.apply([], devs).length;

            }
        )
  }

}
