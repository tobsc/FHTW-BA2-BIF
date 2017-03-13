import { Injectable } from '@angular/core';
import {Http, Headers, RequestOptions, Response} from "@angular/http";
import {User} from "../shared/user.model";
import {Observable} from "rxjs";


@Injectable()
export class AuthService {

  private token: string;
  private loggedIn: boolean = false;
  private url: string = "/api/auth/";

  constructor(private http: Http) {

    this.loggedIn = !!localStorage.getItem('auth_token');

    if (this.isLoggedIn()) {
      this.token = localStorage.getItem('auth_token');
    }
  }

  public login(user: User): Observable<boolean> {
    let bodyString = JSON.stringify(user);
    let headers = new Headers({'Content-Type': 'application/json'});
    let options = new RequestOptions({headers: headers});
    return this.http.post(this.url + 'login/', bodyString, options)
        .map((response: Response) => {
          let token = response.json() && response.json().token;
          if(token) {
            this.token = token;
            this.loggedIn = true;
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
    this.token = null;
    localStorage.removeItem('auth_token');
    this.loggedIn = false;
  }

}
