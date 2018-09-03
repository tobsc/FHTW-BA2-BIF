import { Injectable } from '@angular/core';
import { JwtHttpService } from "./jwt-http.service";
import { Headers, RequestOptions, Response, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs";
import { Damage } from "../models/damage.model";
import { DamageStatus } from "../models/damage-status.model";

@Injectable()
export class DamageService {
    private url: string = '/api/damages/';

    constructor(
        private http: JwtHttpService)
    { }

    public getDamages(): Observable<Damage[]> {
        return this.http.get(this.url)
            .map((response: Response) => response.json());
    }

    public getDamageStati(): Observable<DamageStatus[]> {
        return this.http.get(this.url + 'damagestatus/')
            .map((response: Response) => response.json());
    }

    public getDamagesByInvNum(invnum: string,
        params: URLSearchParams = new URLSearchParams()
    ): Observable<Damage[]> {
        params.set('InvNum', invnum);
        let options = new RequestOptions({
            search: params,
        }); 
        return this.http.get(this.url + 'invnum/', options)
            .map((response: Response) => response.json());
    }

    public getDamagesByDeviceId(deviceid: number,
        params: URLSearchParams = new URLSearchParams()
    ): Observable<Damage[]> {
        params.set('deviceid', deviceid.toString());
        let options = new RequestOptions({
            search: params,
        }); 
        return this.http.get(this.url + 'deviceid/', options)
            .map((response: Response) => response.json());
}

    public getDamage(id: number): Observable<Damage> {
        return this.http.get(this.url + 'id/'+ id)
            .map((response: Response) => response.json());
    }

    public updateDamage(damage: Damage): Observable<Damage> {
        let bodyString = JSON.stringify(damage);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this.http.put(this.url + 'id/' + damage.DamageId, bodyString, options)
            .map((response: Response) => response.json());
    }

    public createDamage(damage: Damage): Observable<Damage> {
        let bodyString = JSON.stringify(damage);
        let headers = new Headers({
            'Content-Type': 'application/json'
        });
        let options = new RequestOptions({ headers: headers });
        return this.http.post(this.url, bodyString, options)
            .map((response: Response) => response.json());
    }
}
