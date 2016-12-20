import { Injectable } from '@angular/core';
import {Device} from "../devices/shared/device.model";

@Injectable()
export class CartService {

  private items: Device[] = [];

  constructor() {
    if (!!localStorage.getItem('cart_list')) {
      this.items = JSON.parse(localStorage.getItem('cart_list'));
    }
  }

  public getItems() {
    return this.items;
  }

  public addItem(item: Device) {
    if (this.items.indexOf(item) < 0) {
      this.items.push(item);
      this.updateLocalStorage();
    }
  }

  public removeItem(index: number) {
    this.items.splice(index, 1);
    this.updateLocalStorage();
  }

  private updateLocalStorage(): void {
    localStorage.setItem('cart_list', JSON.stringify(this.items));
  }
}
