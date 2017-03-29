import {Component, OnInit, OnDestroy} from '@angular/core';
import {DeviceService} from "../../shared/services/device.service";
import {ActivatedRoute} from "@angular/router";
import {Subscription, Observable} from "rxjs";
import { Device } from "../../shared/models/device.model";
import { CartService } from "../../shared/services/cart.service";
import {DeviceMeta} from "../../shared/models/device-meta.model";
import {IDictionary} from "../../shared/common/dictionary.interface";
import {Dictionary} from "../../shared/common/dictionary.class";

@Component({
  selector: 'hwinf-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit, OnDestroy {

  private subscription: Subscription;
  private currentType: string;
  private devices: Device[];

  constructor(
      private deviceService: DeviceService,
      private cartService: CartService,
      private route: ActivatedRoute
  ) { }

  ngOnInit() {
      this.subscription = this.route.params
        .map((params) => params['type'])
        .flatMap((type) => this.deviceService.getDevices(type))
        .subscribe((data) => {
            this.devices = data;
        });
  }

  public addItem(device: Device) {
      this.cartService.addItem(device);
  }

  public getMetaDataOfFieldGroup(slug: string, metaData: DeviceMeta[]): DeviceMeta[] {
      return metaData.filter((i) => i.FieldGroupSlug === slug);
  }

  ngOnDestroy(): void {
      this.subscription.unsubscribe();  
  }

}
