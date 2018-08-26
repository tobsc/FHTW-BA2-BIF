import { Component, OnInit } from '@angular/core';
import {OrderService} from "../../shared/services/order.service";
import {OrderFilter} from "../../shared/models/order-filter.model";
import {OrderItem} from "../../shared/models/order-item.model";
import {Order} from "../../shared/models/order.model";
import {OrderList} from "../../shared/models/order-list.model";
import {ActivatedRoute} from "@angular/router";
import {Filter} from "../../shared/models/filter.model";
import {Observable} from "rxjs";

@Component({
  selector: 'hwinf-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit {
  public currentStatus = '';
  public filter: OrderFilter;

  constructor(
      public orderService: OrderService,
      public route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.route.params
        .map(i => i['status'])
        .subscribe(
            status => {
              let filter = new OrderFilter();
              filter.StatusSlugs = (status === 'archiv') ? ['abgeschlossen', 'abgelehnt'] : [status];
              this.currentStatus = status;
              this.filter = filter;
            }
        );
  }

}
