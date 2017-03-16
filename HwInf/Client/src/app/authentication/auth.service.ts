import { Injectable } from '@angular/core';
import {Http, Headers, RequestOptions, Response} from "@angular/http";
import {User} from "../shared/models/user.model";
import {Observable} from "rxjs";
import { Router } from "@angular/router";
import { JwtService } from "../shared/services/jwt.service";


@Injectable()
export class AuthService {

  private token: string;
  private loggedIn: boolean = false;
  private url: string = "/api/auth/";

  constructor(
      private http: Http,
      private router: Router,
      private jwtService: JwtService
  ) {

      this.loggedIn = !!this.jwtService.getToken();

    if (this.isLoggedIn()) {
        this.token = this.jwtService.getToken();
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
            this.jwtService.setToken( token);
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
    this.jwtService.removeToken();
    this.loggedIn = false;
    this.router.navigate(['/login']);
  }

}
