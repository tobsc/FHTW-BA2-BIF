import { Injectable } from '@angular/core';
import { Http, Headers, URLSearchParams, RequestOptions, Response } from "@angular/http";
import { User } from "../models/user.model";
import { Observable } from "rxjs";
import { JwtService } from "./jwt.service";
import { CartService } from "./cart.service";
import { FeedbackHttpService } from './feedback-http.service';
import { JwtHttpService } from "./jwt-http.service";
import { Router } from "@angular/router";
import { Setting } from "../models/setting.model";


@Injectable()
export class AdminService {

    private token: string;
    private loggedIn: boolean = false;
    private authUrl: string = "/api/auth/";
    private settingsUrl: string = "api/settings/";
    private settings: Setting[];

    constructor(
        private jwtService: JwtService,
        private http: JwtHttpService,
        private cartService: CartService,
        private router:Router
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

                    //badhack
                    this.router.navigate(["/dashboard"]);
                    location.reload();
                }
            });
    }

    public getSetting(key: string): Observable<Setting> {

        let setting = this.settings.filter(i => i.Key == key)[0];

        if (!!setting) {
            return Observable.of(setting);
        }
        else {
            return this.http.get(this.settingsUrl + key)
                .map((response: Response) => response.json());
        }
    }

    public updateSetting(body: Setting): Observable<Setting> {
        let bodyString = JSON.stringify(body);
        let headers = new Headers({
            'Content-Type': 'application/json'
        });
        let options = new RequestOptions({ headers: headers });
        return this.http.put(this.settingsUrl, bodyString, options)
            .do(i => {
                if (i.status == 200) {
                    sessionStorage.setItem(body.Key, body.Value);
                }
            })
            .map((response: Response) => response.json()).do(i => {
            });
    }

    public updateSettings(body: Setting[]): Observable<Setting[]> {
        let bodyString = JSON.stringify(body);
        let headers = new Headers({
            'Content-Type': 'application/json'
        });
        let options = new RequestOptions({ headers: headers });
        return this.http.put(this.settingsUrl+"/multiple", bodyString, options)
            .do(i => {
                if (i.status == 200) {
                    for (let set of body) {
                        sessionStorage.setItem(set.Key, set.Value);
                    }
                }
            })
            .map((response: Response) => response.json()).do(i => {
            });
    }

    public loadSemestreData() {
        return this.http.get(this.settingsUrl)
            .map((response: Response) => response.json())
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

    public getSettings(): Setting[] {
        return this.settings;
    }
    
}
