import { Component, OnInit } from '@angular/core';
import {DeviceService} from "../device.service";

@Component({
  selector: 'hw-inf-device-add',
  templateUrl: './device-add.component.html',
  styleUrls: ['./device-add.component.scss']
})
export class DeviceAddComponent implements OnInit {

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
  }

}
