import { Injectable } from '@angular/core';
import {JwtHttpService} from "./jwt-http.service";
import {User} from "./user.model";
import {Response} from "@angular/http";
import {Observable} from "rxjs";

@Injectable()
export class UserService {
  private user: Observable<any> = null;
  private url: string = '/api/users/';

  constructor(private http: JwtHttpService) {}

  public getUser(): Observable<User> {
    if (this.user === null) {
      this.user = this.http.get(this.url + 'userdata')
        .map((response: Response) => response.json())
        .cache();
    }

      return this.user;
  }

  public clearCache(): void {
    this.user = null;
  }
}
