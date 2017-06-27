import {Component, OnInit, Input, Output,EventEmitter} from '@angular/core';
import { Order } from "../../../shared/models/order.model";
import {OrderService } from "../../../shared/services/order.service";

@Component({
  selector: 'hwinf-single-order',
  templateUrl: './single-order.component.html',
  styleUrls: ['./single-order.component.scss']
})
export class SingleOrderComponent implements OnInit {

    @Input() order: Order;
    @Output() deleteOrder: EventEmitter<any> = new EventEmitter<any>();



  constructor(
      private orderService: OrderService
  ) {
      
  }

   public onAbort(): void {
        this.orderService.abortOrder(this.order).subscribe(
            (success) => { this.deleteOrder.emit(null); },
            (error) => console.log(error)
        );
    }

    ngOnInit() {
  }

}
