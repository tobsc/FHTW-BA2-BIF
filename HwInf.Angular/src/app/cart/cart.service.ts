import { Injectable } from '@angular/core';
import {Device} from "../devices/shared/device.model";

@Injectable()
export class CartService {

  private items: Device[] = [];

  constructor() { }

  public getItems() {
    return this.items;
  }

  public addItem(item: Device) {
    if (this.items.indexOf(item) < 0) {
      this.items.push(item);
    }
    console.log(this.items);
  }

  public removeItem(item: Device) {
    this.items.splice(this.items.indexOf(item), 1);
  }
}
