import {Component, OnInit, AfterViewInit} from '@angular/core';
import {AuthService} from "./authentication/auth.service";
import {Router} from "@angular/router";
import {CartService} from "./cart/cart.service";
import {AuthGuard} from "./authentication/auth.guard";
import {UserService} from "./shared/user.service";
import {User} from "./shared/user.model";
import {Observable} from "rxjs";

@Component({
  selector: 'hw-inf-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent implements OnInit {

  private cartAmount: number = 0;
  private user: Observable<User>;

  constructor(private authService: AuthService,
              private router: Router,
              private cartService: CartService,
              private userService: UserService) { }

  ngOnInit() {
    this.cartService.getAmount().subscribe((data: number) => {
      this.cartAmount = data;
    });
    this.cartService.updateAmount();

    this.user = this.userService.getUser();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}
