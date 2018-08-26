import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {DeviceType} from "../models/device-type.model";
import {Response} from "@angular/http";
import { Device } from "../models/device.model";
import { IDictionary } from "../../shared/common/dictionary.interface";
import { Dictionary } from "../../shared/common/dictionary.class";
import { DeviceComponent } from "../models/component.model";
import {DeviceList} from "../models/device-list.model";
import {Status} from "../models/status.model";
import {Accessory} from "../models/accessory.model";
import {HttpClient} from "@angular/common/http";
import { HttpHeaders, HttpParams } from "@angular/common/http";
import "rxjs";

@Injectable()
export class DeviceService {

    public url: string = '/api/devices/';
    public deviceTypes: Observable<string[]> = null;
    public deviceComponents: IDictionary<Observable<DeviceComponent[]>> = new Dictionary<Observable<DeviceComponent[]>>();


    constructor(public http: HttpClient) { }

    /**
     * Returns Devices of given type
     * empty type string returns all types
     * @param type DeviceType.TypeName
     * @param quantity
     * @param offset
     * @param params
     * @returns {Observable<Device[]>}
     */
    public getDevices(
        type: string = "",
        limit: number = 100,
        offset: number = 0,
        params: HttpParams = new HttpParams()
    ): Observable<DeviceList> {

        params = params.append('limit', limit + '');
        params = params.append('offset', offset + '');

      return this.http.get<DeviceList>(this.url + type + '/', { params });
    }

    public getSearch(
        searchText: string = "",
        limit: number = 100,
        offset: number = 0,
        params: HttpParams = new HttpParams()
    ): Observable<DeviceList> {

        params = params.append('searchText', searchText + '');
        params = params.append('limit', limit + '');
        params = params.append('offset', offset + '');

      return this.http.get<DeviceList>(this.url + 'search', { params });
    }

    public getFilteredDevices(body: any): Observable<DeviceList> {
        let bodyString = JSON.stringify(body);
        let headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
      return this.http.post<DeviceList>(this.url + 'filter', bodyString, { headers });
    }

    public getFilteredDevicesUser(body: any): Observable<DeviceList> {
        let bodyString = JSON.stringify(body);
        let headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
      return this.http.post<DeviceList>(this.url + 'filteruser', bodyString, { headers });
    }

    public getDevice(invNum: string,
        params: HttpParams = new HttpParams()
    ): Observable<Device> {
        params = params.append('InvNum', invNum);

      return this.http.get<Device>(this.url + 'invnum/', { params });
    }

    public getDeviceById(id: number): Observable<Device> {
        return this.http.get<Device>(this.url + 'id/'+id)
    }

    public getDeviceStatuses(): Observable<Status[]> {
        return this.http.get<Status[]>(this.url + 'status')
    }

    /**
     * Returns all device types. e.g Notebook, PC, Monitor, ...
     * @returns {Observable<DeviceType[]>}
     */
    public getDeviceTypes(showEmptyDeviceTypes : boolean = true): Observable<DeviceType[]> {
        let params: HttpParams = new HttpParams();
        params = params.append('showEmptyDeviceTypes', showEmptyDeviceTypes + '');

      return this.http.get<DeviceType[]>(this.url + 'types', { params });
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
            this.http.get<DeviceComponent[]>(this.url + 'types/' + type)
            );
        }
        return this.deviceComponents.get(type);
    }

    public addDeviceType(body: DeviceType): Observable<DeviceType> {
        let bodyString = JSON.stringify(body);
        console.log(bodyString);
        let headers = new HttpHeaders({
           'Content-Type': 'application/json'
        });
      return this.http.post<DeviceType>(this.url + 'types', bodyString, { headers });
    }

    public addNewDevice(body: Device): Observable<Device> {
        let bodyString = JSON.stringify(body);
        let headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
      return this.http.post<Device>(this.url, bodyString, { headers });
    }

    public editDevice(body: Device): Observable<Device> {
        let bodyString = JSON.stringify(body);
        let headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
      return this.http.put<Device>(this.url + "id/" + body.DeviceId, bodyString, { headers });
    }

    public deleteDevice(id: number) {
        return this.http.delete(this.url +"id/" + id)
    }

    public addNewDeviceType(body: DeviceType): Observable<DeviceType> {
        let bodyString = JSON.stringify(body);
        let headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
      return this.http.post<DeviceType>(this.url + "types/", bodyString, { headers });
    }

    public editDeviceType(body: DeviceType): Observable<DeviceType> {
        let bodyString = JSON.stringify(body);
        let headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
      return this.http.put<DeviceType>(this.url + "types/" + body.Slug, bodyString, { headers });
    }

    public deleteDeviceType(slug: string) {
        return this.http.delete(this.url + "types/" + slug);
    }

    public getAccessories(): Observable<Accessory[]> {
        return this.http.get<Accessory[]>(this.url + 'accessories/')
    }

    public addAccessory(body: Accessory): Observable<Accessory> {
        let bodyString = JSON.stringify(body);
        let headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
      return this.http.post<Accessory>(this.url + 'accessories', bodyString, { headers });
    }

    public deleteAccessory(slug: string) {
        return this.http.delete(this.url +"accessories/" + slug);
    }

    public updateAccessory(body: Accessory): Observable<Accessory> {
        let bodyString = JSON.stringify(body);
        let headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
      return this.http.put<Accessory>(this.url + "accessories/" + body.Slug, bodyString, { headers });
    }


}
