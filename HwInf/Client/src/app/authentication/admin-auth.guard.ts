import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import {AuthService} from "./auth.service";
import {UserService} from "../shared/user.service";

@Injectable()
export class AdminAuthGuard implements CanActivate {

  privato

  constructor(private router: Router,
              private authService: AuthService,
              private userService: UserService) { }

  canActivate() {
    if (this.authService.isLoggedIn() == false) {
      this.router.navigate(['/login']);
    }

    return this.authService.isLoggedIn();
  }

}
