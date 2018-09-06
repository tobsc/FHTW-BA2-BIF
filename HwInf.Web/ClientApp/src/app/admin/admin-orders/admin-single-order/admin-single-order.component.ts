import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {Order} from "../../../shared/models/order.model";
import {OrderService} from "../../../shared/services/order.service";
import {OrderItem} from "../../../shared/models/order-item.model";
import {BehaviorSubject} from "rxjs";

@Component({
  selector: 'hwinf-admin-single-order',
  templateUrl: './admin-single-order.component.html',
  styleUrls: ['./admin-single-order.component.scss']
})
export class AdminSingleOrderComponent implements OnInit {


  @Input() order: Order;
  @Output() updateOrder: EventEmitter<Order> = new EventEmitter<Order>();

  constructor(
      private orderService: OrderService
  ) { }

  ngOnInit(): void {
    console.log(this.order);
  }

  onAccept(): void {
    this.orderService.acceptOrder(this.order).subscribe(
        (success) => { this.updateOrder.emit(success); },
        (error)   => console.log(error)
    );
  }

  canAccept(): boolean {
    return this.order.OrderItems.filter(i => !i.IsDeclined).length > 0;
  }

  onDecline(): void {
    this.orderService.declineOrder(this.order).subscribe(
        (success) => { this.updateOrder.emit(success); },
        (error)   => console.log(error)
    );
  }

  onLend(): void {
    this.orderService.lendOrder(this.order).subscribe(
        (success) => { this.updateOrder.emit(success); },
        (error)   => console.log(error)
    );
  }

  onReset(): void {
    this.orderService.resetOrder(this.order).subscribe(
        (success) => { this.updateOrder.emit(success); },
        (error)   => console.log(error)
    );
  }

  onReturn(): void {
    this.orderService.returnOrder(this.order).subscribe(
        (success) => { this.updateOrder.emit(success); },
        (error)   => console.log(error)
    );
  }
}
