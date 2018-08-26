import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {Order} from "../models/order.model";
import {OrderFilter} from "../models/order-filter.model";
import {OrderItem} from "../models/order-item.model";
import {OrderList} from "../models/order-list.model";
import {HttpClient} from "@angular/common/http";
import {HttpHeaders} from "@angular/common/http";

@Injectable()
export class OrderService {

    public readonly url: string = '/api/orders/';
    public readonly printUrl: string = "/api/print/";

  constructor(public http: HttpClient) { }



  public getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.url)
  }

  public getFilteredOrders(body: OrderFilter): Observable<OrderList> {
    let bodyString = JSON.stringify(body);
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.post<OrderList>(this.url + 'filter', bodyString, {headers})
  }

  public updateOrderItem(id: number, statusSlug: string): Observable<OrderItem> {
    let bodyString = JSON.stringify("");
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.put<OrderItem>(`${this.url}orderitem/${id}/${statusSlug}`, bodyString, {headers})
  }



  public createOrder(body: any): Observable<Order> {
    let bodyString = JSON.stringify(body);
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.post<Order>(this.url, bodyString, {headers})
  }

  public acceptOrder(body: Order): Observable<Order> {
    let bodyString = JSON.stringify(body);
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.put<Order>(`${this.url}order/accept`, bodyString, {headers})
  }

  public lendOrder(body: any): Observable<Order> {
    let bodyString = JSON.stringify(body);
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.put<Order>(`${this.url}order/lend`, bodyString, {headers})
  }

  public resetOrder(body: any): Observable<Order> {
    let bodyString = JSON.stringify(body);
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.put<Order>(`${this.url}order/reset`, bodyString, {headers})
  }

  public declineOrder(body: any): Observable<Order> {
    let bodyString = JSON.stringify(body);
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.put<Order>(`${this.url}order/decline`, bodyString, {headers})
  }

  public abortOrder(body: any): Observable<Order> {
      let bodyString = JSON.stringify(body);
      let headers = new HttpHeaders({
          'Content-Type': 'application/json'
      });
      return this.http.put<Order>(`${this.url}order/abort`, bodyString, {headers})
  }

  public returnOrder(body: any): Observable<Order> {
    let bodyString = JSON.stringify(body);
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.put<Order>(`${this.url}order/return`, bodyString, {headers})
  }
  public printOrder(orderGuid: string) {
      window.open(this.printUrl + orderGuid);
  }

  public printReturn(orderGuid: string) {
      window.open(this.printUrl + 'return/' + orderGuid);
  }
}
