import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import {CartService} from "../../../../shared/services/cart.service";

@Component({
  selector: 'hwinf-order-confirm',
  templateUrl: './order-confirm.component.html',
  styleUrls: ['./order-confirm.component.scss']
})
export class OrderConfirmComponent implements OnInit {

  constructor(
      private cartService: CartService,
      private router: Router
  ) { }

  ngOnInit() {
      this.cartService.clear();

      setTimeout((router: Router) => {
          this.router.navigate(['/']);
      }, 15000);  //15s
  }

}
