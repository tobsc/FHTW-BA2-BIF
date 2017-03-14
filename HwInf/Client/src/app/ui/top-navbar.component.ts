import { Component, OnInit } from '@angular/core';
import {AppComponent} from "../app.component";
import {AuthService} from "../login/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'hwinf-top-navbar',
  templateUrl: './top-navbar.component.html',
  styleUrls: ['./top-navbar.component.scss']
})
export class TopNavbarComponent implements OnInit {

  private isCollapsed: boolean = false;
  constructor(
      private rootComp: AppComponent,
      private authService: AuthService) {  }

  setClass() {
    this.isCollapsed = !this.isCollapsed;
    if ( this.isCollapsed ){
      this.rootComp.cssClass = 'nav-sm';
    }
    else {
      this.rootComp.cssClass = 'nav-md';
    }
  }

  ngOnInit() {

  }

  public logout(): void {
    this.authService.logout();
  }

}
