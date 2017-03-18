import {Component, OnInit} from '@angular/core';
import {Observable} from "rxjs";
import {DeviceType} from "../../../../shared/models/device-type.model";
import {DeviceService} from "../../../../shared/services/device.service";
import {Device} from "../../../../shared/models/device.model";

@Component({
  selector: 'hwinf-device-types-list',
  templateUrl: './device-types-list.component.html',
  styleUrls: ['./device-types-list.component.scss']
})
export class DeviceTypesListComponent implements OnInit {

  private deviceTypes: DeviceType[] = [];

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    this.deviceService.getDeviceTypes()
        .subscribe((data) => {
          this.deviceTypes = data;
        });
  }

  pushData(item: DeviceType) {
    this.deviceTypes.push(item);
  }

}
