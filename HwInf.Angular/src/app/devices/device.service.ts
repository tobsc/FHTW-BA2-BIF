import { Injectable } from '@angular/core';
import {Device} from "./Device.class";
import {Http, Response, URLSearchParams} from "@angular/http";
import 'rxjs/Rx';
import {Dictionary} from "../shared/Dictionary";
import {Observable} from "rxjs";
import {DeviceComponent} from "./DeviceComponent.class";

@Injectable()
export class DeviceService {
    private devices = new Dictionary<Device>();

    constructor(private http: Http) {}

    getDevices(type: string = "", params: URLSearchParams = null): Observable<Device[]> {
        return this.http.get('http://localhost:14373/api/devices/' + type + '/', { search: params })
            .map((response: Response) => response.json());
    }

    getDevice(id: number): Observable<Device[]> {
        return this.http.get('http://localhost:14373/api/devices/id/' + id)
            .map((response: Response) => response.json());
    }

    getTypes(): Observable<string[]> {
        return this.http.get('http://localhost:14373/api/devices/types/')
            .map((response: Response) => response.json());
    }

    getComponents(type: string): Observable<DeviceComponent[]> {
        return this.http.get('http://localhost:14373/api/devices/components/' + type)
            .map((response: Response) => response.json());
    }

    getComponentValues(type: string, component: string): Observable<string[]> {
        return this.http.get('http://localhost:14373/api/devices/components/' + type + '/' + component.toLowerCase())
            .map((response: Response) => response.json());
    }
}
