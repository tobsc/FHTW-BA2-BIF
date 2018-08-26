import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";

@Injectable()
export class OrderProcessService {

  public stati: Array<BehaviorSubject<string>> = [];

  constructor() {
    this.stati.push(new BehaviorSubject<string>("selected"));
    this.stati.push(new BehaviorSubject<string>("disabled"));
    this.stati.push(new BehaviorSubject<string>("disabled"));
  }


  getStatus(index: number): Observable<string> {
    return this.stati[index].asObservable();
  }

  setStatus(index: number, status: string) {
    this.stati[index].next(status);
  }

}
