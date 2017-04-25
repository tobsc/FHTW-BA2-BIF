import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {OrderItem} from "../../../shared/models/order-item.model";
import {OrderService} from "../../../shared/services/order.service";
var moment = require('moment');
moment.locale('de');


@Component({
  selector: 'hwinf-admin-order-list',
  templateUrl: './admin-order-list.component.html',
  styleUrls: ['./admin-order-list.component.scss']
})
export class AdminOrderListComponent implements OnInit {

  @Input() private orderItems: OrderItem[];
  @Output() private updateList: EventEmitter<string> = new EventEmitter();
  constructor(private orderService: OrderService) { }

  ngOnInit() {
  }

  getReturnDate(date: string): string {
    let d = moment(date);
    return ( d.year() === 1) ? '--' : d.format('DD. MMMM YYYY');
  }

  onAccept(orderId: number): void {
    this.orderService.updateOrderItem(orderId, 'akzeptiert')
        .subscribe(
          (next) => {
            console.log(next);
            this.updateList.emit(null);
          },
          (err)  => console.log(err)
        );
  }

  onDecline(orderId: number): void {
    this.orderService.updateOrderItem(orderId, 'abgelehnt')
        .subscribe(
            (next) => {
              console.log(next);
              this.updateList.emit(null);
            },
            (err)  => console.log(err)
        );
  }

  onReturn(orderId: number): void {
    this.orderService.updateOrderItem(orderId, 'abgeschlossen')
        .subscribe(
            (next) => {
              console.log(next);
              this.updateList.emit(null);
            },
            (err)  => console.log(err)
        );
  }
}
