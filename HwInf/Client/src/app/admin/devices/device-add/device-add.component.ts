
import {Component, OnInit} from "@angular/core";
import {DeviceService} from "../../../shared/services/device.service";
import {Device} from "../../../shared/models/device.model";
@Component({
  selector: 'hwinf-device-add',
  templateUrl: './device-add.component.html',
  styleUrls: ['./device-add.component.scss']
})
export class DeviceAddComponent implements OnInit {
  constructor(
      private deviceService: DeviceService
  ) { }

  ngOnInit() {
  }

  onSubmit(device: Device) {
    this.deviceService.addNewDevice(device).subscribe(
        (next) => { console.log(next) },
        (error) => { console.log(error) }
    );
  }

}
