import {Component, OnInit, Input} from '@angular/core';
import {OrderItem} from "../../../shared/models/order-item.model";

@Component({
  selector: 'hwinf-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.scss']
})
export class OrderListComponent implements OnInit {

  @Input() private orderItems: OrderItem[];

  constructor() {}

  ngOnInit() {

  }

}
