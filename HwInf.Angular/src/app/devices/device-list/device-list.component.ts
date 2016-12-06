import { Component, OnInit } from '@angular/core';
import {Device} from "../device";
import {DeviceService} from "../device.service";

@Component({
  selector: 'hw-inf-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit {

  private devices: Device[] = [];
  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
    this.deviceService.getDevices()
        .subscribe(
            (data: Device[]) => {
              this.devices = data;
            }
        );
  }
}
