import {Component, Input} from '@angular/core';
import {Device} from "../shared/device.model";
import {CartService} from "../../cart/cart.service";

@Component({
  selector: 'hw-inf-device-item',
  templateUrl: './device-item.component.html',
  styleUrls: ['./device-item.component.scss']
})
export class DeviceItemComponent {
  @Input() device: Device;

  constructor(private cartService: CartService) {}

  public onAddToCartClick(item: Device) {
    this.cartService.addItem(item);
  }
}
