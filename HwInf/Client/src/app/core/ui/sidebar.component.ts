import { Component, OnInit } from '@angular/core';
import {DeviceService} from "../../shared/services/device.service";
import {Observable} from "rxjs";
import {DeviceType} from "../../shared/models/device-type.model";
import {Router} from "@angular/router";

@Component({
  selector: 'hwinf-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {

  private deviceTypes: Observable<DeviceType[]>;

  constructor(
      private deviceService: DeviceService,
      private router: Router
) { }

  ngOnInit() {
    this.deviceTypes = this.deviceService.getDeviceTypes();
  }

}
