import { Component, OnInit } from '@angular/core';
import {OrderService} from "../../../shared/services/order.service";
import {OrderFilter} from "../../../shared/models/order-filter.model";
import {Order} from "../../../shared/models/order.model";

@Component({
  selector: 'hwinf-orders-archiv',
  templateUrl: './orders-archiv.component.html',
  styleUrls: ['./orders-archiv.component.scss']
})
export class OrdersArchivComponent implements OnInit {

  private orders: Order[];

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {

    let filter = new OrderFilter();
    filter.StatusSlugs = ['abgeschlossen', 'abgelehnt'];

    this.orderService.getFilteredOrders(filter)
        .subscribe(
            data => { this.orders = data.Orders;console.log(data); }
        )
  }
}
