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
      .map((response: Response) => response.json());
  }
}
