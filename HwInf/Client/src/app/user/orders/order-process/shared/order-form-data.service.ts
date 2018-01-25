import { Injectable } from '@angular/core';
import { Order } from "../../../../shared/models/order.model";
import { OrderItem } from "../../../../shared/models/order-item.model";

@Injectable()
export class OrderFormDataService {

  private orderMdl: Order = new Order();

  constructor() { }

  public getData(): Order {
    return this.orderMdl;
  }


  public setData(mdl: Order): void {
    this.orderMdl = mdl;
  }

  public getOrderItems(): OrderItem[] {
      return this.orderMdl.OrderItems;
  }

  public reset() {
    this.orderMdl = new Order();
  }

}
