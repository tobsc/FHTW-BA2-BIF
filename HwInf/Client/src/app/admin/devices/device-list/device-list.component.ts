import {Component, OnInit, OnDestroy } from "@angular/core";
import {DeviceService} from "../../../shared/services/device.service";
import {Subscription} from "rxjs";
import {Device} from "../../../shared/models/device.model";
import {ActivatedRoute, Router} from "@angular/router";
import {DeviceList} from "../../../shared/models/device-list.model";

@Component({
  selector: 'hwinf-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit, OnDestroy {

  private currentPage: number = 1;
  private maxPages: number = 0;
  private subscription: Subscription;
  private devices: Device[];

  constructor(
      private deviceService: DeviceService,
      private route: ActivatedRoute,
      private router: Router
  ) {}

  ngOnInit() {

      this.fetchData();
  }

  fetchData() {
      this.subscription = this.route.params
          .subscribe(
              (params: any) => {

                  if ( params['page'] ) {
                      let pagenumber = +params['page'];
                      if (pagenumber > 0) {
                          this.currentPage = pagenumber ;

                      }
                  }

                  console.log("currentpage: " + this.currentPage);
                  console.log("offset: " + (this.currentPage-1) *  2);


                  this.deviceService.getDevices("", 100, (this.currentPage-1) *  100 ).subscribe(
                      (data: DeviceList) => {
                          this.maxPages = data.MaxPages;
                          this.devices = data.Devices;

                          console.log(this.devices);
                      }
                  )
              }
          );
  }

  removeDevice(index: number) {
      this.devices.splice(index, 1);
  }

  onDelete( deviceId: number, index: number) {

      this.deviceService.deleteDevice(deviceId)
          .subscribe(
              () => { this.removeDevice(index) },
              (err) => console.log(err)
          );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
