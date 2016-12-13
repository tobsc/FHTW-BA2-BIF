import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import {User} from "./user.model";
import {AuthService} from "./auth.service";

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private authService: AuthService) { }

  canActivate() {
    if (this.authService.isLoggedIn() == false) {
      this.router.navigate(['/login']);
    }
    return this.authService.isLoggedIn();

/*    if (localStorage.getItem('auth_token')) {
      // logged in so return true
      return true;
    }

    // not logged in so redirect to login page
    this.router.navigate(['/login']);
    return false;*/
  }

}
