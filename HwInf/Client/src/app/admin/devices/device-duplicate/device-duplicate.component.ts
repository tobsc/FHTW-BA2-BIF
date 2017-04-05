import { Component, OnInit } from '@angular/core';
import {Device} from "../../../shared/models/device.model";
import {DeviceService} from "../../../shared/services/device.service";

@Component({
  selector: 'hwinf-device-duplicate',
  templateUrl: './device-duplicate.component.html',
  styleUrls: ['./device-duplicate.component.scss']
})
export class DeviceDuplicateComponent implements OnInit {

  public alerts: any = [];
  constructor(
      private deviceService: DeviceService,
  ) { }

  ngOnInit() {
  }

  onSubmit(device: Device) {
    this.deviceService.addNewDevice(device).subscribe(
        (next) => {
          this.alerts.push({
            type: 'success',
            msg: `Das Gerät <strong>${next.Name}</strong> wurde erfolgreich hinzugefügt!`,
            timeout: 5000
          });
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
