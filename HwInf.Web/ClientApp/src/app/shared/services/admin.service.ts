import { Injectable } from '@angular/core';
import { Headers, RequestOptions, Response } from "@angular/http";
import { User } from "../models/user.model";
import { Observable } from "rxjs";
import { JwtService } from "./jwt.service";
import { CartService } from "./cart.service";
import { Router } from "@angular/router";
import { Setting } from "../models/setting.model";
import {HttpClient} from "@angular/common/http";
import {HttpHeaders} from "@angular/common/http";
import "rxjs/Rx";

@Injectable()
export class AdminService {

    public token: string;
    public loggedIn: boolean = false;
    public authUrl: string = "/api/auth/";
    public settingsUrl: string = "/api/settings/";
    public userUrl: string = "/api/users/";
    public settings: Setting[];

    constructor(
        public jwtService: JwtService,
        public http: HttpClient,
        public cartService: CartService,
        public router:Router
    ) {
        this.token = jwtService.getToken();
    }

    public impersonate(user: User){
        this.http.get(this.authUrl + 'impersonate/' + user.Uid + '/')
            .map((response: Response) => response.json())
            .subscribe((data) => {
                let token = data.token;
                if (token) {
                    this.token = token;
                    this.loggedIn = true;
                    this.jwtService.removeToken();
                    this.jwtService.setToken(token);

                    location.reload();
                }
            });
    }

    public addAdmin(user: User) {

        return this.http.get(this.userUrl + "admin/" + user.Uid);

    }

    public removeAdmin(user: User, role: string) {

        return this.http.get(this.userUrl + "admin/remove/" + user.Uid + "/" + role);
    }

    public getSetting(key: string): Observable<Setting> {

        let setting = this.settings.filter(i => i.Key == key)[0];

        if (!!setting) {
            return Observable.of(setting);
        }
        else {
          return this.http.get<Setting>(this.settingsUrl + key);
        }
    }

    public updateSetting(body: Setting): Observable<Setting> {
        let bodyString = JSON.stringify(body);
        return this.http.put<Setting>(this.settingsUrl, bodyString)
            .do(i => {
              sessionStorage.setItem(body.Key, body.Value);
            });
    }

    public updateSettings(body: Setting[]): Observable<Setting[]> {
        let bodyString = JSON.stringify(body);
        let httpOptions = {
          headers: new HttpHeaders({
            'Content-Type':  'application/json',
          })
        };
      return this.http.put<Setting[]>(this.settingsUrl + "multiple", bodyString, httpOptions)
        .do(i => {
          for (let set of body) {
            sessionStorage.setItem(set.Key, set.Value);
          }
        });
    }

    public loadSemestreData() {
        return this.http.get<Setting[]>(this.settingsUrl)
            .subscribe(setting => {
                this.settings = setting;
                if (sessionStorage.getItem("settings") != "true") {
                  console.log("set storage");
                  for (let set of this.settings) {
                        sessionStorage.setItem(set.Key, set.Value);
                    }
                    sessionStorage.setItem("settings", "true");
                }
            });
    }


    public getLog(): Observable<string[]>  {
        return this.http.get<string[]>(this.settingsUrl + 'logs/')
    }

    public getSettings(): Setting[] {
        return this.settings;
    }

}
