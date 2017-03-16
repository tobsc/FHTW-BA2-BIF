import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from "@angular/router";
import { AuthService } from "./auth.service";

@Injectable()
export class AdminGuard implements CanActivate {

    constructor(private router: Router, private authService: AuthService) { }

    canActivate() {

        if (!this.authService.isLoggedIn()) {
            this.router.navigate(['/login']);
            return false;
        }

        //console.log(this.parseJwt(localStorage.getItem('auth_token')));

        console.log(this.getRole(localStorage.getItem('auth_token')));

        if (this.getRole(localStorage.getItem('auth_token')).toLowerCase() === 'admin') {
            return true;
        } else {
            return false;
        }
    }

    

    public parseJwt(token): any {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace('-', '+').replace('_', '/');
        return JSON.parse(window.atob(base64));
    };

    public getRole(token): string {
        return this.parseJwt(token).role;
    }
}
