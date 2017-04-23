import { Injectable } from '@angular/core';
import {Order} from "../../../../shared/models/order.model";

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

  public reset() {
    this.orderMdl = new Order();
  }

}
