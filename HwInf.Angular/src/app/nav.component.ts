import {Component, OnInit, AfterViewInit} from '@angular/core';
import {AuthService} from "./authentication/auth.service";
import {Router} from "@angular/router";
import {CartService} from "./cart/cart.service";

@Component({
  selector: 'hw-inf-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent implements AfterViewInit {

  private cartAmount: number = 0;

  constructor(private authService: AuthService,
              private router: Router,
              private cartService: CartService) { }

  ngAfterViewInit() {

    this.cartService.getAmount().subscribe((data: number) => {
      this.cartAmount = data;
    });

    this.cartService.updateAmount();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}
