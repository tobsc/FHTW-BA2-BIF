import { Injectable } from '@angular/core';
import { JwtHttpService } from "./jwt-http.service";
import { Headers, RequestOptions, Response } from "@angular/http";
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

    public getDamagesByInvNum(invnum: string): Observable<Damage[]> {
        return this.http.get(this.url + 'invnum/' + invnum)
            .map((response: Response) => response.json());
    }

    public getDamage(id: number): Observable<Damage> {
        return this.http.get(this.url + 'id/'+ id)
            .map((response: Response) => response.json());
    }

    public updateDamage(damage: Damage): Observable<boolean> {
        let bodyString = JSON.stringify(damage);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this.http.put(this.url + 'id/' + damage.DamageId, bodyString, options)
            .map((response: Response) => {
                let token = response.json() && response.json().token;
                if (token) {
                    return true;
                } else {
                    return false;
                }
            });
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
