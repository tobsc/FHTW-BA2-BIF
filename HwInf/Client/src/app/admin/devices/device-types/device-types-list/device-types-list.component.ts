import { Component, OnInit } from '@angular/core';
import {Observable} from "rxjs";
import {DeviceType} from "../../../../shared/models/device-type.model";
import {DeviceService} from "../../../../shared/services/device.service";

@Component({
  selector: 'hwinf-device-types-list',
  templateUrl: './device-types-list.component.html',
  styleUrls: ['./device-types-list.component.scss']
})
export class DeviceTypesListComponent implements OnInit {

  private deviceTypes: Observable<DeviceType[]> = null;

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
    this.deviceTypes = this.deviceService.getDeviceTypes();
  }

}
