import { Component, OnInit } from '@angular/core';
import {OrderService} from "../../shared/services/order.service";
import {OrderFilter} from "../../shared/models/order-filter.model";
import {OrderItem} from "../../shared/models/order-item.model";
import {Order} from "../../shared/models/order.model";
import {OrderList} from "../../shared/models/order-list.model";
import {ActivatedRoute} from "@angular/router";
import {Filter} from "../../shared/models/filter.model";

@Component({
  selector: 'hwinf-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit {

  private filter: OrderFilter;

  constructor(
      private orderService: OrderService,
      private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.queryParams.map(i => i['status']).subscribe(status => {
      this.filter = new OrderFilter();
      if (!!status)
      this.filter.StatusSlugs.push(status);
    });
  }
}
