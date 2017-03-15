import { Injectable } from '@angular/core';
import {JwtHttpService} from "./jwt-http.service";
import {Observable} from "rxjs";
import {DeviceType} from "../models/device-type.model";
import {Response, URLSearchParams, RequestOptions} from "@angular/http";
import { Device } from "../models/device.model";
import { IDictionary } from "../../shared/common/dictionary.interface";
import { Dictionary } from "../../shared/common/dictionary.class";
import { DeviceComponent } from "../models/component.model";

@Injectable()
export class DeviceService {

    private url: string = '/api/devices/';
    private deviceTypes: Observable<string[]> = null;
    private deviceComponents: IDictionary<Observable<DeviceComponent[]>> = new Dictionary<Observable<DeviceComponent[]>>();


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

  /**
 * Returns objects of type DeviceComponent, which holds the component name e.g Prozessor
 * and all values present in the database
 * @param type
 * @returns {Observable<DeviceComponent[]>}
 */
  public getComponentsAndValues(type: string): Observable<DeviceComponent[]> {
      if (!this.deviceComponents.containsKey(type)) {
          this.deviceComponents.add(
              type,
              this.http.get(this.url + 'types/' + type)
                  .map((response: Response) => response.json())
          );
      }
      return this.deviceComponents.get(type);
  }

}
