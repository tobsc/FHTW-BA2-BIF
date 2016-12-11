import { Injectable } from '@angular/core';
import {Device} from "./device.class";
import {Http, Response, URLSearchParams, Headers, RequestOptions} from "@angular/http";
import 'rxjs/Rx';
import {Observable} from "rxjs";
import {DeviceComponent} from "./device-component.class";
import {IDictionary} from "../shared/IDictionary";
import {Dictionary} from "../shared/Dictionary";

@Injectable()
export class DeviceService {
    private url: string = '/api/devices/';
    private deviceTypes: Observable<string[]> = null;
    private deviceComponents: IDictionary<Observable<DeviceComponent[]>> = new Dictionary<Observable<DeviceComponent[]>>();

    constructor(private http: Http) {}

    getDevices(type: string = "", params: URLSearchParams = null): Observable<Device[]> {
        return this.http.get(this.url + type + '/', { search: params })
            .map((response: Response) => response.json());
    }

    getDevice(id: number): Observable<Device[]> {
        return this.http.get(this.url + 'id/' + id)
            .map((response: Response) => response.json());
    }

    getTypes(): Observable<string[]> {
        if ( this.deviceTypes === null) {
            this.deviceTypes = this.http.get(this.url + 'types/')
                .map((response: Response) => response.json());
        }
        return this.deviceTypes;
    }

    getComponents(type: string): Observable<DeviceComponent[]> {
      if (!this.deviceComponents.containsKey(type)) {
        this.deviceComponents.add(
          type,
          this.http.get(this.url +'components/' + type)
            .map((response: Response) => response.json())
        );
      }
      return this.deviceComponents.get(type);
    }

    addDevice(body: Device): Observable<Device> {
      let bodyString = JSON.stringify(body);
      let headers = new Headers({'Content-Type': 'application/json'});
      let options = new RequestOptions({headers: headers});
      return this.http.post(this.url + 'create/', bodyString, options)
        .map((response: Response) => response.json());
    }
    //http://localhost:14373/api/devices/components/pc/prozessor/In
    getComponentValues(type: string, component: string, term: string) {
      return this.http.get(this.url + 'components/' + type + '/' + term)
        .map((response: Response) => response.json());
    }
}
