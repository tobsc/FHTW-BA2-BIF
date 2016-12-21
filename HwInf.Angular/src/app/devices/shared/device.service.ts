import { Injectable } from '@angular/core';
import {Device} from "./device.model";
import {Http, Response, URLSearchParams, Headers, RequestOptions} from "@angular/http";
import 'rxjs/Rx';
import {Observable} from "rxjs";
import {DeviceComponent} from "./device-component.model";
import {IDictionary} from "../../shared/dictionary.interface";
import {Dictionary} from "../../shared/dictionary.class";
import {AuthService} from "../../authentication/auth.service";
import {JwtHttpService} from "../../shared/jwt-http.service";

@Injectable()
export class DeviceService {
  private url: string = '/api/devices/';
  private deviceTypes: Observable<string[]> = null;
  private deviceComponents: IDictionary<Observable<DeviceComponent[]>> = new Dictionary<Observable<DeviceComponent[]>>();

  constructor(private http: JwtHttpService, private authService: AuthService) {}

  /**
   * Get devices of given type with given search params
   * @param type  e.g. notebook, pc, etc.
   * @param params  components of devices e.g. processor, ram, etc.
   * @returns {Observable<Device[]>}
   */
  public getDevices(type: string = "", params: URLSearchParams = null): Observable<Device[]> {
    let options = new RequestOptions({
      search: params,
    });
    return this.http.get(this.url + type + '/', options)
      .map((response: Response) => response.json());
  }

  /**
   * Get single object of type device
   * @param id ID of device in database
   * @returns {Observable<Device[]>} should return an array of size 1
   */
  public getDevice(id: number): Observable<Device[]> {
    return this.http.get(this.url + 'id/' + id)
      .map((response: Response) => response.json());
  }

  /**
   * @returns {Observable<string[]>} all device types
   */
  public getTypes(): Observable<string[]> {
    if ( this.deviceTypes === null) {
      this.deviceTypes = this.http.get(this.url + 'types/')
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
  public getComponentsAndValues(type: string): Observable<DeviceComponent[]> {
    if (!this.deviceComponents.containsKey(type)) {
      this.deviceComponents.add(
        type,
        this.http.get(this.url +'components/' + type)
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
  public addDevice(body: Device): Observable<Device> {
    let bodyString = JSON.stringify(body);
    let headers = new Headers({
      'Content-Type': 'application/json'
    });
    let options = new RequestOptions({headers: headers});
    return this.http.post(this.url + 'create/', bodyString, options)
      .map((response: Response) => response.json());
  }
  //http://localhost:14373/api/devices/components/pc/prozessor/In
  public getComponentValues(type: string, component: string, term: string): Observable<string[]> {
    return this.http.get(this.url + 'components/' + type + '/' + term)
      .map((response: Response) => response.json());
  }

  public clearCache(): void {
    this.deviceTypes = null;
    this.deviceComponents= new Dictionary<Observable<DeviceComponent[]>>();
  }
}
