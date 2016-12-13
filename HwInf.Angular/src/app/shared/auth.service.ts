import { Injectable } from '@angular/core';
import {Http, Headers, RequestOptions, Response} from "@angular/http";
import {Observable} from "rxjs";
import {User} from "./user.model";

@Injectable()
export class AuthService {

  private loggedIn: boolean = false;

  private url: string = '/api/Auth/';

  constructor(private http: Http) {
    this.loggedIn = !!localStorage.getItem('auth_token');
  }

  public login(user: User): Observable<boolean> {
    let bodyString = JSON.stringify(user);
    let headers = new Headers({'Content-Type': 'application/json'});
    let options = new RequestOptions({headers: headers});
    return this.http.post(this.url + 'SignIn/', bodyString, options)
      .map((response: Response) => {
        let token = response.json() && response.json().token;
        if(token) {
          localStorage.setItem('auth_token', token);
          return true;
        } else {
          return false;
        }
      });
  }

  public isLoggedIn(): boolean {
    return this.loggedIn;
  }

  public logout(): void {
    localStorage.removeItem('auth_token');
    this.loggedIn = false;
  }
}
