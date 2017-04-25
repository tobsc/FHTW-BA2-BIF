import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../shared/services/order.service';
import { Order } from '../../shared/models/order.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'hwinf-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit {

    private orderService: OrderService;
    private orders: Observable<Order[]>;

    constructor() {
       
    }

    ngOnInit() {
        console.log(this.orderService.getOrders());
  }

}
