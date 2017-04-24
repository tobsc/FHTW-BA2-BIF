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

    public getSetting(key:string): Observable<Setting> {
        return this.http.get(this.settingsUrl + key)
            .map((response: Response) => response.json());
    }
}
