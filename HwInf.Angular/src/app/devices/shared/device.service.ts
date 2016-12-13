import { Injectable } from '@angular/core';
import {Device} from "./device.model";
import {Http, Response, URLSearchParams, Headers, RequestOptions} from "@angular/http";
import 'rxjs/Rx';
import {Observable} from "rxjs";
import {DeviceComponent} from "./device-component.model";
import {IDictionary} from "../../shared/dictionary.interface";
import {Dictionary} from "../../shared/dictionary.class";
import {AuthService} from "../../shared/auth.service";

@Injectable()
export class DeviceService {
  private url: string = '/api/devices/';
  private deviceTypes: Observable<string[]> = null;
  private deviceComponents: IDictionary<Observable<DeviceComponent[]>> = new Dictionary<Observable<DeviceComponent[]>>();

  constructor(private http: Http, private authService: AuthService) {}

  /**
   * Get devices of given type with given search params
   * @param type  e.g. notebook, pc, etc.
   * @param params  components of devices e.g. processor, ram, etc.
   * @returns {Observable<Device[]>}
   */
  getDevices(type: string = "", params: URLSearchParams = null): Observable<Device[]> {
    let headers = new Headers({'Authorization': 'Bearer ' + this.authService.getToken()});
    let options = new RequestOptions({
      search: params,
      headers: headers
    });
    return this.http.get(this.url + type + '/', options)
      .map((response: Response) => response.json());
  }

  /**
   * Get single object of type device
   * @param id ID of device in database
   * @returns {Observable<Device[]>} should return an array of size 1
   */
  getDevice(id: number): Observable<Device[]> {
    let headers = new Headers({'Authorization': 'Bearer ' + this.authService.getToken()});
    let options = new RequestOptions({
      headers: headers
    });
    return this.http.get(this.url + 'id/' + id, options)
      .map((response: Response) => response.json());
  }

  /**
   * @returns {Observable<string[]>} all device types
   */
  getTypes(): Observable<string[]> {
    if ( this.deviceTypes === null) {
      let headers = new Headers({'Authorization': 'Bearer ' + this.authService.getToken()});
      let options = new RequestOptions({
        headers: headers
      });
      this.deviceTypes = this.http.get(this.url + 'types/', options)
        .map((response: Response) => response.json())
        .cache();
    }
    return this.deviceTypes;
  }

  /**
   * Returns objects of type DeviceComponent, which holds the component name e.g Prozessor
   * and all values present in the database
   * @param type
   * @returns {Observable<DeviceComponent[]>}
   */
  getComponentsAndValues(type: string): Observable<DeviceComponent[]> {
    if (!this.deviceComponents.containsKey(type)) {
      let headers = new Headers({'Authorization': 'Bearer ' + this.authService.getToken()});
      let options = new RequestOptions({
        headers: headers
      });


      this.deviceComponents.add(
        type,
        this.http.get(this.url +'components/' + type, options)
          .map((response: Response) => response.json())
          .cache()
      );
    }
    return this.deviceComponents.get(type);
  }

  /**
   * Add a device to the database
   * @param body object of type Device
   * @returns {Observable<Device>}
   */
  addDevice(body: Device): Observable<Device> {
    let bodyString = JSON.stringify(body);
    let headers = new Headers({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + this.authService.getToken()
    });
    let options = new RequestOptions({headers: headers});
    return this.http.post(this.url + 'create/', bodyString, options)
      .map((response: Response) => response.json());
  }
  //http://localhost:14373/api/devices/components/pc/prozessor/In
  getComponentValues(type: string, component: string, term: string): Observable<string[]> {
    return this.http.get(this.url + 'components/' + type + '/' + term)
      .map((response: Response) => response.json());
  }
}
