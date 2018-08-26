import { Injectable } from '@angular/core';
import {CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot} from "@angular/router";
import {AuthService} from "./auth.service";

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(public router: Router, public authService: AuthService) { }

  canActivate() {

      if (this.authService.isLoggedIn()) {
          return true;
      } else {
          this.authService.logout();
          return false;
      }
  }

}
