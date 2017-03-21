import { Injectable } from '@angular/core';
import { JwtHttpService } from "./jwt-http.service";
import { Http, Headers, RequestOptions, Response } from "@angular/http";
import {User} from "../models/user.model";
import { Observable } from "rxjs";
import { Router } from "@angular/router";
import { JwtService } from "./jwt.service";

@Injectable()
export class UserService {
    private user: Observable<any> = null;
    private url: string = '/api/users/';
    private token: string;


    constructor(
        private http: JwtHttpService,
        private router: Router,
        private jwtService: JwtService) {}

    public getUser(): Observable<User> {
        if (this.user === null) {
            this.user = this.http.get(this.url + 'userdata')
                .map((response: Response) => response.json());
        }
        return this.user;
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
                    console.log(token);
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
