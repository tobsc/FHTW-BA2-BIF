import { Component, OnInit } from '@angular/core';
import {DeviceService} from "./device.service";
import {Response} from "@angular/http";

@Component({
  selector: 'hw-inf-devices',
  templateUrl: './devices.component.html',
  providers: [DeviceService]
})
export class DevicesComponent implements OnInit {

  constructor(private deviceService: DeviceService) {}

  ngOnInit() {
  }

}
