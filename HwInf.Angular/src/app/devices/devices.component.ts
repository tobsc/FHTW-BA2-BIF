import { Component, OnInit } from '@angular/core';
import {DeviceService} from "./shared/device.service";
import {Response} from "@angular/http";

@Component({
  selector: 'hw-inf-devices',
  templateUrl: './devices.component.html',
})
export class DevicesComponent implements OnInit {

  constructor(private deviceService: DeviceService) {}

  ngOnInit() {
  }

}
