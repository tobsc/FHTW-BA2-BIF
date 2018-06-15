import { Injectable } from '@angular/core';
import { Response } from "@angular/http";
import { Observable } from "rxjs";
import { Damage } from "../models/damage.model";
import { DamageStatus } from "../models/damage-status.model";
import {HttpClient} from "@angular/common/http";
import {HttpParams} from "@angular/common/http";
import {HttpHeaders} from "@angular/common/http";

@Injectable()
export class DamageService {
    private url: string = '/api/damages/';

    constructor(
        private http: HttpClient)
    { }

    public getDamages(): Observable<Damage[]> {
      return this.http.get<Damage[]>(this.url);
    }

    public getDamageStati(): Observable<DamageStatus[]> {
      return this.http.get<DamageStatus[]>(this.url + 'damagestatus/');
    }

    public getDamagesByInvNum(invnum: string,
        params: HttpParams = new HttpParams()
    ): Observable<Damage[]> {
        params = params.set('InvNum', invnum);

      return this.http.get<Damage[]>(this.url + 'invnum/', { params });
    }

    public getDamagesByDeviceId(deviceid: number,
        params: HttpParams = new HttpParams()
    ): Observable<Damage[]> {
       params = params.set('deviceid', deviceid.toString());

      return this.http.get<Damage[]>(this.url + 'deviceid/', { params });
    }

    public getDamage(id: number): Observable<Damage> {
      return this.http.get<Damage>(this.url + 'id/' + id);
    }

    public updateDamage(damage: Damage): Observable<Damage> {
        let bodyString = JSON.stringify(damage);
        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
      return this.http.put<Damage>(this.url + 'id/' + damage.DamageId, bodyString, { headers });
    }

    public createDamage(damage: Damage): Observable<Damage> {
        let bodyString = JSON.stringify(damage);
        let headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
      return this.http.post<Damage>(this.url, bodyString, { headers });
    }
}
