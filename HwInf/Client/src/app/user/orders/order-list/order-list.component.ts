import { Component, OnInit } from '@angular/core';
import {OrderService} from "../../../shared/services/order.service";
import {Order} from "../../../shared/models/order.model";
import {Observable} from "rxjs";

@Component({
  selector: 'hwinf-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.scss']
})
export class OrderListComponent implements OnInit {

  private orders: Observable<Order[]>;

  constructor(private orderService: OrderService) { }

  ngOnInit() {
    this.orders = this.orderService.getOrders();
  }

}
