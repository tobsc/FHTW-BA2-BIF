import { Component, OnInit } from '@angular/core';
import {DeviceService} from "../devices/device.service";
import {Device} from "../devices/device.class";
import {Observable} from "rxjs";

@Component({
  selector: 'hw-inf-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  private test: Observable<Device[]>;

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
    this.test = this.deviceService.getDevices('pc');
  }

}
