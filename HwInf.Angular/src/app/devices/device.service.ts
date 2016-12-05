import { Injectable } from '@angular/core';
import {Device} from "./device";
import {Http, Response} from "@angular/http";
import 'rxjs/Rx';
import {Dictionary} from "../shared/Dictionary";

@Injectable()
export class DeviceService {
  private devices = new Dictionary<Device>();

  constructor(private http: Http) {}

  getDevices() {
      return this.http.get('http://localhost:14373/api/devices/all')
      .map((response: Response) => response.json());
  }

  getDevice(id: number) {
    return this.http.get('http://localhost:14373/api/devices/' + id)
        .map((response: Response) => response.json());
  }


}
