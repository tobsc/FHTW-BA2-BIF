import {Component, OnInit, AfterViewInit} from '@angular/core';
import {AuthService} from "./authentication/auth.service";
import {Router} from "@angular/router";
import {CartService} from "./cart/cart.service";
import {AuthGuard} from "./authentication/auth.guard";

@Component({
  selector: 'hw-inf-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent implements OnInit {

  private cartAmount: number = 0;

  constructor(private authService: AuthService,
              private router: Router,
              private cartService: CartService) { }

  ngOnInit() {
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
