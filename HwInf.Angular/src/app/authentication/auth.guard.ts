import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import {UserCredentials} from "../shared/user-credentials.model";
import {AuthService} from "./auth.service";

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private authService: AuthService) { }

  canActivate() {
    if (this.authService.isLoggedIn() == false) {
      this.router.navigate(['/login']);
    }
    return this.authService.isLoggedIn();
  }

}
