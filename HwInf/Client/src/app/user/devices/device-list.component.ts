import { Component, OnInit } from '@angular/core';
import {DeviceService} from "../../shared/services/device.service";
import {ActivatedRoute} from "@angular/router";
import {Subscription, Observable} from "rxjs";
import { Device } from "../../shared/models/device.model";
import { CartService } from "../../shared/services/cart.service";

@Component({
  selector: 'hwinf-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit {
  private subscription: Subscription;
  private currentType: string;
  private devices: Observable<Device[]>;

  constructor(
      private deviceService: DeviceService,
      private cartService: CartService,
      private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.subscription = this.route.params
        .subscribe(
            (params: any) => {

              this.currentType = params['type'];
              this.devices = this.deviceService.getDevices(this.currentType);
            }
        );
  }

  public addItem(device: Device) {
      console.log("following object has been recieved");
      console.log(device);
      this.cartService.addItem(device);
      
  }

}
