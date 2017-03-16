import { Component, OnInit } from '@angular/core';
import {AuthService} from "../../authentication/auth.service";
import { AppComponent } from "../../app.component";
import { CartService } from "../../shared/services/cart.service";

@Component({
  selector: 'hwinf-top-navbar',
  templateUrl: './top-navbar.component.html',
  styleUrls: ['./top-navbar.component.scss']
})
export class TopNavbarComponent implements OnInit {

  private isCollapsed: boolean = false;
  constructor(
      private authService: AuthService,
      private rootComp: AppComponent,
      private cartService: CartService
  ) {  }

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
