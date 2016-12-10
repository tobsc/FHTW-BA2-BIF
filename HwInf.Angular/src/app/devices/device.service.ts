import { Injectable } from '@angular/core';
import {Device} from "./device.class";
import {Http, Response, URLSearchParams} from "@angular/http";
import 'rxjs/Rx';
import {Observable} from "rxjs";
import {DeviceComponent} from "./device-component.class";

@Injectable()
export class DeviceService {
    private types: Observable<string[]> = null;
    private url: string = '/api/devices/';

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
        if ( this.types === null) {
            this.types = this.http.get(this.url + 'types/')
                .map((response: Response) => response.json());
        }
        return this.types;
    }

    getComponents(type: string): Observable<DeviceComponent[]> {
       return this.http.get(this.url +'components/' + type)
            .map((response: Response) => response.json());
    }
}
