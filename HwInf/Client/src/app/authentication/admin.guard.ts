import { Injectable } from '@angular/core';
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from "@angular/router";
import { AuthService } from "./auth.service";
import { JwtService } from "../shared/services/jwt.service";

@Injectable()
export class AdminGuard implements CanActivate {

    constructor(private router: Router, private authService: AuthService, private jwtService: JwtService) { }

    canActivate() {

        if (!this.authService.isLoggedIn()) {
            this.router.navigate(['/login']);
            return false;
        }

        //console.log(this.parseJwt(localStorage.getItem('auth_token')));

        console.log(this.jwtService.getRole());

        if (this.jwtService.isAdmin()) {
            return true;
        } else {
            return false;
        }
    }

    

   
    
}
