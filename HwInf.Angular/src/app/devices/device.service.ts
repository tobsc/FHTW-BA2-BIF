import { Injectable } from '@angular/core';
import {Device} from "./device";
import {Http, Response} from "@angular/http";
import 'rxjs/Rx';
import {Dictionary} from "../shared/Dictionary";

@Injectable()
export class DeviceService {
  private devices = new Dictionary<Device>();

  constructor(private http: Http) {}

  fetchData() {
    this.http.get('api/devices/all')
      .map((response: Response) => response.json())
      .subscribe(
        (data: any) => {
          for (let device of data) {
            this.devices.add(
              String(device.DeviceId),
              new Device(
                device.DeviceId,
                device.Name,
                device.InvNum,
                device.Status,
                device.TypeName,
                device.DeviceMetaData
              )
            );
          }
          console.log(this.devices.values());
        }
      );

    return this.devices;
  }

  getDevices() {
      return this.http.get('api/devices/all')
      .map((response: Response) => response.json());
  }

  getDevice(id: number): Device {
    return null;
  }


}
