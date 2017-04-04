import { Injectable } from '@angular/core';
import { JwtHttpService } from "./jwt-http.service";
import { Headers, RequestOptions, Response } from "@angular/http";
import {User} from "../models/user.model";
import { Observable } from "rxjs";


@Injectable()
export class UserService {
    private url: string = '/api/users/';
    private token: string;


    constructor(
        private http: JwtHttpService) {}

    public getUser(): Observable<User> {
        return this.http.get(this.url + 'userdata')
            .map((response: Response) => response.json());
    }

    public updateUser(user: User): Observable<boolean> {
        let bodyString = JSON.stringify(user);
        console.log(bodyString);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this.http.post(this.url + 'update', bodyString, options)
            .map((response: Response) => {
                let token = response.json() && response.json().token;
                if (token) {
                    return true;
                } else {
                    return false;
                }

            });
    }

    public getOwners(): Observable<User[]> {
        return this.http.get(this.url + '/owners')
            .map((response: Response) => response.json());
    }
}
