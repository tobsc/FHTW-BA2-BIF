import {Component, OnInit, OnDestroy} from '@angular/core';
import {DeviceService} from "../../../shared/services/device.service";
import {Observable, Subscription} from "rxjs";
import {Device} from "../../../shared/models/device.model";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'hwinf-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit, OnDestroy {

  private currentPage: number = 0;
  private subscription: Subscription;
  private devices: Observable<Device[]>;

  constructor(
      private deviceService: DeviceService,
      private route: ActivatedRoute,
      private router: Router
  ) { }

  ngOnInit() {
    this.subscription = this.route.params
        .subscribe(
            (params: any) => {

              if ( params['page'] ) {
                this.currentPage = +params['page'] ;
              }
              this.devices = this.deviceService.getDevices("", 1, this.currentPage);
            }
        );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
