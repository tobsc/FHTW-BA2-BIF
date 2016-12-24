import { Injectable } from '@angular/core';
import {JwtHttpService} from "../shared/jwt-http.service";
import {Order} from "./order.model";
import {Observable} from "rxjs";
import {Headers, RequestOptions, Response} from "@angular/http";

@Injectable()
export class OrderService {

  private url = '/api/orders/';

  constructor(private http: JwtHttpService) { }

  public createOrder(body: Order): Observable<string> {
    let bodyString = JSON.stringify(body);
    let headers = new Headers({
      'Content-Type': 'application/json'
    });
    let options = new RequestOptions({headers: headers});
    return this.http.post(this.url + 'create/', bodyString, options)
      .map((response: Response) => {
        console.log(response);
        return response.json()
      });
  }

  public getOutgoingOrders(): Observable<Order[]> {
    return this.http.get(this.url + 'outgoing/')
      .map((response: Response) => response.json());
  }

  public getIncomingOrders(): Observable<Order[]> {
    return this.http.get(this.url + 'incoming/')
      .map((response: Response) => response.json());
  }

  public acceptOrder(id: number) {
    return this.http.get(this.url + '/id/' + id +'/accept')
      .map((response: Response) => response.json());
  }

  public declineOrder(id: number) {
    return this.http.get(this.url + '/id/' + id +'/decline')
      .map((response: Response) => response.json());
  }
}
