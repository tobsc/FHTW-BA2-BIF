import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from "@angular/router";
import {CartService} from "../../../../shared/services/cart.service";

@Component({
  selector: 'hwinf-order-confirm',
  templateUrl: './order-confirm.component.html',
  styleUrls: ['./order-confirm.component.scss']
})
export class OrderConfirmComponent implements OnInit, OnDestroy {

    public timer;

  constructor(
      public cartService: CartService,
      public router: Router
  ) { }

  ngOnInit() {
      this.cartService.clear();

      this.timer = setTimeout((router: Router) => {
          this.router.navigate(['/']);
      }, 10000);  //10s
  }

  ngOnDestroy() {

      clearTimeout(this.timer);;
  }
}
