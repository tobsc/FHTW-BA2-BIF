import { Injectable } from '@angular/core';
import { Headers, RequestOptions, Response } from "@angular/http";
import {User} from "../models/user.model";
import { Observable } from "rxjs";
import {HttpClient} from "@angular/common/http";
import {HttpHeaders} from "@angular/common/http";


@Injectable()
export class UserService {
    public url: string = '/api/users/';
    public token: string;


    constructor(
        public http: HttpClient) {}

    public getUser(): Observable<User> {
      return this.http.get<User>(this.url + 'userdata');
    }

    public updateUser(user: User): Observable<boolean> {
        let bodyString = JSON.stringify(user);
        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        return this.http.post(this.url + 'update', bodyString, {headers})
            .map((response: Response) => {
                let token = response.json() && response.json().token;
                if (token) {
                    return true;
                } else {
                    return false;
                }
            });
    }

    public getUsers(): Observable<User[]> {
        return this.http.get<User[]>(this.url + 'users')
    }

    public getOwners(): Observable<User[]> {
        return this.http.get<User[]>(this.url + 'owners')
    }

    public getAdmins(): Observable<User[]> {
        return this.http.get<User[]>(this.url + 'admins')
    }

}
