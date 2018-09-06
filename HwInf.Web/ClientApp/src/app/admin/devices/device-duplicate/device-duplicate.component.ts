import { Component, OnInit } from '@angular/core';
import {Device} from "../../../shared/models/device.model";
import { DeviceService } from "../../../shared/services/device.service";
import { Router } from "@angular/router";

@Component({
  selector: 'hwinf-device-duplicate',
  templateUrl: './device-duplicate.component.html',
  styleUrls: ['./device-duplicate.component.scss']
})
export class DeviceDuplicateComponent implements OnInit {

  public alerts: any = [];
  constructor(
      private deviceService: DeviceService,
      private router:Router
  ) { }

  ngOnInit() {
  }

  onSubmit(device: Device) {
    this.deviceService.addNewDevice(device).subscribe(
        (next) => {
          this.router.navigate(["/admin/geraete/verwalten"]);

        },
        (error) => {
          let msg = JSON.parse(error._body).Message;
          this.alerts.push({
            type: 'danger',
            msg: msg,
            timeout: 5000
          });
        }
    );
  }

}
