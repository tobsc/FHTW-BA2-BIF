import { Component, OnInit } from '@angular/core';
import {DeviceType} from "../../../../shared/models/device-type.model";
import {DeviceService} from "../../../../shared/services/device.service";

@Component({
  selector: 'hwinf-device-custom-fields-fieldgroups-list',
  templateUrl: './device-custom-fields-fieldgroups-list.component.html',
  styleUrls: ['./device-custom-fields-fieldgroups-list.component.scss']
})
export class DeviceCustomFieldsFieldgroupsListComponent implements OnInit {

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
