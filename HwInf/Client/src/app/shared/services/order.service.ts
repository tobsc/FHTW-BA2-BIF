import { Injectable } from '@angular/core';
import {JwtHttpService} from "./jwt-http.service";
import {Observable} from "rxjs";
import {Order} from "../models/order.model";
import {Response, RequestOptions, Headers} from "@angular/http";

@Injectable()
export class OrderService {

  private readonly url: string = '/api/orders/';

  constructor(private http: JwtHttpService) { }



  public getOrders(): Observable<Order[]> {
    return this.http.get(this.url)
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
}
