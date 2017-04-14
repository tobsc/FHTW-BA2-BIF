import {Component, OnInit, OnDestroy} from '@angular/core';
import {DeviceService} from "../../shared/services/device.service";
import {ActivatedRoute} from "@angular/router";
import {Subscription, Observable} from "rxjs";
import { Device } from "../../shared/models/device.model";
import { CartService } from "../../shared/services/cart.service";
import {DeviceMeta} from "../../shared/models/device-meta.model";
import {IDictionary} from "../../shared/common/dictionary.interface";
import {Dictionary} from "../../shared/common/dictionary.class";
import {Filter} from "../../shared/models/filter.model";

@Component({
  selector: 'hwinf-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit, OnDestroy {

  private currentPage: number = 1;
  private subscription: Subscription;
  private currentType: string;
  private devices: Device[];
  private filter: Filter;

  constructor(
      private deviceService: DeviceService,
      private cartService: CartService,
      private route: ActivatedRoute
  ) { }

  ngOnInit() {

      console.log('yes');


      this.subscription = this.route.params
        .map((params) => params['type'])
        .do((type) => {
            if (type)
                this.deviceService.getDeviceTypes().map(i => i.filter(x => x.Slug == type)[0]).map(i => i.Name).subscribe(
                    i => {
                        this.currentType = i;
                    }
                );
        })
        .flatMap((type) => {
            this.filter = new Filter();
            this.filter.DeviceType = type;
            this.filter.Order = 'ASC';
            this.filter.OrderBy = 'name';
            this.filter.Limit = 10;
            this.filter.Offset = (this.currentPage-1) * this.filter.Limit;
            return this.deviceService.getFilteredDevices(this.filter)
        })
        .subscribe((data) => {
            this.devices = data.Devices;
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
