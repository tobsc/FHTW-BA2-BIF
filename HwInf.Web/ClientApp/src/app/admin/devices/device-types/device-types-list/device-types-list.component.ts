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
    private rows: any[];
    

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
      this.fetchData();
  }

  fetchData() {
    this.deviceService.getDeviceTypes()
        .subscribe((data) => {
            this.deviceTypes = data;
          
            this.rows = data.map(i => ({ isCollapsed: true, deviceType: i }));
        });
  }

  pushData(deviceType: DeviceType) {
      this.deviceTypes.push(deviceType);
      this.rows.unshift({ isCollapsed: true, deviceType: deviceType });
  }

  removeDeviceType(index: number) {
      this.deviceTypes.splice(index, 1);
      this.rows.splice(index, 1);
  }

  onDelete(typeSlug: string, index: number) {

      this.deviceService.deleteDeviceType(typeSlug)
          .subscribe(
          () => { this.removeDeviceType(index) },
          (err) => console.log(err)
          );
  }

  onSave(i, deviceType) {
      this.deviceService.editDeviceType(deviceType)
          .subscribe(
          (success) => {
           
              this.rows[i].deviceType = success;
              this.rows[i].isCollapsed = true;
          },
           (error) => console.log(error)
      );
  }

 

}
