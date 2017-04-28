import {Component, OnInit} from '@angular/core';
import {DeviceType} from "../../../../shared/models/device-type.model";
import {DeviceService} from "../../../../shared/services/device.service";

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
            console.log(data);
          this.deviceTypes = data;
        });
  }

  pushData(item: DeviceType) {
    this.deviceTypes.push(item);
  }

  removeDeviceType(index: number) {
      this.deviceTypes.splice(index, 1);
  }

  onDelete(typeSlug: string, index: number) {

      this.deviceService.deleteDeviceType(typeSlug)
          .subscribe(
          () => { this.removeDeviceType(index) },
          (err) => console.log(err)
          );
  }

}
