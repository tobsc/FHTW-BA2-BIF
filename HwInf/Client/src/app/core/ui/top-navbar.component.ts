import { Component, OnInit } from '@angular/core';
import {AuthService} from "../../authentication/auth.service";
import { AppComponent } from "../../app.component";
import { CartService } from "../../shared/services/cart.service";
import { UserService } from "../../shared/services/user.service";
import { User } from "../../shared/models/user.model";


@Component({
  selector: 'hwinf-top-navbar',
  templateUrl: './top-navbar.component.html',
  styleUrls: ['./top-navbar.component.scss']
})
export class TopNavbarComponent implements OnInit {
  private user: User;
  private isCollapsed: boolean = false;

  constructor(
      private authService: AuthService,
      private rootComp: AppComponent,
      private cartService: CartService,
      private userService: UserService
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
     this.userService.getUser().subscribe((data) => this.user = data);
  }

  public logout(): void {
    this.authService.logout();
  }


}
