import {Component, OnInit, Input} from '@angular/core';
import {DeviceService} from "../device.service";
import {Device} from "../device";
import {Dictionary} from "../../shared/Dictionary";

@Component({
  selector: 'hw-inf-device-list',
  templateUrl: './device-list.component.html',
})
export class DeviceListComponent implements OnInit {
  devices: Device[] = [];
  devicesDictionary: Dictionary<Device> = new Dictionary<Device>();

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
  this.deviceService.getDevices()
    .subscribe(
      (data: any) => {
        for (let device of data) {
          this.devicesDictionary.add(
            String(device.DeviceId),
            new Device(
              device.DeviceId,
              device.Name,
              device.InvNum,
              device.Status,
              device.TypeName,
              device.DeviceMetaData
            )
          );
        }
        this.devices = this.devicesDictionary.values();
      }
    );
  }

}
