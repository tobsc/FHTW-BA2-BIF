import { Component, OnInit } from '@angular/core';
import {AuthService} from "./authentication/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'hw-inf-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}
