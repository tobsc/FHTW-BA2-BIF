import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../shared/services/order.service';
import { Order } from '../../shared/models/order.model';
import { OrderItem } from '../../shared/models/order-item.model';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'hwinf-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {

    
    constructor() { }

    ngOnInit() {
       
    }

}
