import { Component, OnInit } from '@angular/core';
import {Device} from "../../../shared/models/device.model";
import {DeviceService} from "../../../shared/services/device.service";

@Component({
  selector: 'hwinf-device-edit',
  templateUrl: './device-edit.component.html',
  styleUrls: ['./device-edit.component.scss']
})
export class DeviceEditComponent implements OnInit {

  constructor(
      private deviceService: DeviceService
  ) { }

  ngOnInit() {
  }

  onSubmit(device: Device) {
    this.deviceService.editDevice(device).subscribe(
        (next) => { console.log(next) },
        (error) => { console.log(error) }
    );
  }

}