import { Injectable } from '@angular/core';
import {JwtHttpService} from "./jwt-http.service";
import {Observable} from "rxjs";
import {Order} from "../models/order.model";
import {Response, RequestOptions, Headers} from "@angular/http";
import {OrderFilter} from "../models/order-filter.model";
import {OrderItem} from "../models/order-item.model";

@Injectable()
export class OrderService {

    private readonly url: string = '/api/orders/';
    private readonly printUrl: string = "/api/print/";

  constructor(private http: JwtHttpService) { }



  public getOrders(): Observable<Order[]> {
    return this.http.get(this.url)
        .map((response: Response) => response.json());
  }

  public getFilteredOrders(body: OrderFilter = null): Observable<OrderItem[]> {
    let bodyString = !!body ? JSON.stringify(body) : '{}';
    let headers = new Headers({
      'Content-Type': 'application/json'
    });
    let options = new RequestOptions({headers: headers});
    return this.http.post(this.url + 'filter', bodyString, options)
        .map((response: Response) => response.json());
  }

  public updateOrderItem(id: number, statusSlug: string): Observable<OrderItem> {
    let bodyString = JSON.stringify("");
    let headers = new Headers({
      'Content-Type': 'application/json'
    });
    let options = new RequestOptions({headers: headers});
    return this.http.put(`${this.url}orderitem/${id}/${statusSlug}`, bodyString, options)
        .map((response: Response) => response.json());
  }


  public createOrder(body: any): Observable<Order> {
    let bodyString = JSON.stringify(body);
    let headers = new Headers({
      'Content-Type': 'application/json'
    });
    let options = new RequestOptions({headers: headers});
    return this.http.post(this.url, bodyString, options)
        .map((response: Response) => response.json());
  }

  public printOrder(orderGuid: string) {
      window.open(this.printUrl + orderGuid);
  }

  public printReturn(orderGuid: string) {
      window.open(this.printUrl + 'return/' + orderGuid);
  }
}
