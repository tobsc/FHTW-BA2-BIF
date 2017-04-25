import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {Order} from "../../../shared/models/order.model";
import {OrderService} from "../../../shared/services/order.service";

@Component({
  selector: 'hwinf-admin-single-order',
  templateUrl: './admin-single-order.component.html',
  styleUrls: ['./admin-single-order.component.scss']
})
export class AdminSingleOrderComponent implements OnInit {
 

  private undoStack: any[] = [this.reset()];

  @Input() order: Order;
  @Output() updateOrder: EventEmitter<Order> = new EventEmitter<Order>();

  constructor(
      private orderService: OrderService
  ) { }

  ngOnInit(): void {
    console.log("ON INIT");
  }

  private accept() {
    return () => {
      this.orderService.acceptOrder(this.order).subscribe(
          (success) => { this.updateOrder.emit(success); },
          (error)   => console.log(error)
      );
    }
  }

  private reset() : any  {
    return () => {
      this.orderService.resetOrder(this.order).subscribe(
          (success) => { this.updateOrder.emit(success); },
          (error)   => console.log(error)
      );
    }
  }

  onAccept(): void {
    this.accept()();
    this.undoStack.push(this.accept());
    console.log(this.undoStack);
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
