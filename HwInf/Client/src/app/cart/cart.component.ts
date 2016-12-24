import {Component, OnInit, trigger, state, style, transition, animate} from '@angular/core';
import {CartService} from "./cart.service";
import {Device} from "../devices/shared/device.model";

@Component({
  selector: 'hw-inf-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss'],
  animations: [
    trigger('list1', [
      transition('* => void', [
        animate(200, style({
          opacity: 0,
          transform: 'translateX(100px)'
        }))
      ])
    ]),
  ]
})
export class CartComponent implements OnInit {

  private items: Device[];

  constructor(private cartService: CartService) { }

  ngOnInit() {
    this.items = this.cartService.getItems();
  }

  public removeItem(index: number) {
    this.cartService.removeItem(index);
  }

}
