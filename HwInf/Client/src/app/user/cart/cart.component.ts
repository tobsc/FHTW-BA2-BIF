import { Component, OnInit } from '@angular/core';
import { Subscription, Observable } from "rxjs";
import { Device } from "../../shared/models/device.model";
import { CartService } from "../../shared/services/cart.service";


@Component({
  selector: 'hwinf-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {

    private devices: any[];

    constructor(
        private cartService: CartService
    ) { }


    ngOnInit() {
        this.devices = this.cartService.getItems();
        //console.log(this.devices);
        //console.log(this.cartService.getItems());
  }

}
