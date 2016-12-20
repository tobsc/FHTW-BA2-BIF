import { Injectable } from '@angular/core';
import {Device} from "../devices/shared/device.model";
import {Subject, Observable} from "rxjs";

@Injectable()
export class CartService {
  private amount: Subject<number> = new Subject<number>();
  private items: Device[] = [];

  constructor() {
    if (!!localStorage.getItem('cart_list')) {
      this.items = JSON.parse(localStorage.getItem('cart_list'));
    }
    this.amount = new Subject<number>();
    this.updateData();
  }

  public getItems() {
    return this.items;
  }

  public getAmount(): Observable<number> {
    return this.amount.asObservable();
  }

  public addItem(item: Device) {
    if (this.items.indexOf(item) < 0) {
      this.items.push(item);
      this.updateData()
    }
  }

  public removeItem(index: number) {
    this.items.splice(index, 1);
    this.updateData();
  }

  private updateData(): void {
    this.updateLocalStorage();
    this.updateAmount();
  }

  private updateLocalStorage(): void {
    localStorage.setItem('cart_list', JSON.stringify(this.items));
  }

  public updateAmount(): void {
    this.amount.next(this.items.length);
  }
}
