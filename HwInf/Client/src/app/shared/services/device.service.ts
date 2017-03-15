import { Injectable } from '@angular/core';
import {JwtHttpService} from "./jwt-http.service";
import {Observable} from "rxjs";
import {DeviceType} from "../models/device-type.model";
import {Response, URLSearchParams, RequestOptions} from "@angular/http";
import {Device} from "../models/device.model";

@Injectable()
export class DeviceService {

  private url: string = '/api/devices/';

  constructor(private http: JwtHttpService) { }

  public getDevices(
      type: string = "",
      quantity: number = 100,
      offset: number = 0,
      params: URLSearchParams = null
  ): Observable<Device[]> {
    let options = new RequestOptions({
      search: params,
    });

    return this.http.get(this.url + type + '/', options)
        .map((response: Response) => response.json());
  }

  public getDeviceTypes(): Observable<DeviceType[]> {
    return this.http.get(this.url + 'types/')
        .map((response: Response) => response.json());
  }

}
