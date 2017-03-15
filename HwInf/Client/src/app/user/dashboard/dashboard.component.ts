import { Component, OnInit } from '@angular/core';
import {DeviceService} from "../../shared/services/device.service";
import {Observable} from "rxjs";
import {DeviceType} from "../../shared/models/device-type.model";

@Component({
  selector: 'hwinf-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  private deviceTypes: Observable<DeviceType[]>;

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {

    this.deviceTypes = this.deviceService.getDeviceTypes();
  }

}
