import {Component, OnInit, Input} from '@angular/core';
import {Order} from "../../../shared/models/order.model";

@Component({
  selector: 'hwinf-single-order',
  templateUrl: './single-order.component.html',
  styleUrls: ['./single-order.component.scss']
})
export class SingleOrderComponent implements OnInit {

  @Input() order: Order;

  constructor() { }

  ngOnInit() {
  }

}
