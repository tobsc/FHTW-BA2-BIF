
import {Component, OnInit} from "@angular/core";
import {DeviceService} from "../../../shared/services/device.service";
import {Device} from "../../../shared/models/device.model";
import {ErrorHandlerService} from "../../../shared/services/error-handler.service";
@Component({
  selector: 'hwinf-device-add',
  templateUrl: './device-add.component.html',
  styleUrls: ['./device-add.component.scss']
})
export class DeviceAddComponent implements OnInit {

  public alerts: any = [];
  constructor(
      private deviceService: DeviceService,
  ) { }

  ngOnInit() {
  }

  onSubmit(device: Device) {
    this.deviceService.addNewDevice(device).subscribe(
        (next) => {
          this.alerts.push({
            type: 'success',
            msg: `Das Gerät ${next.Name} wurde erfolgreich hinzugefügt!`,
            timeout: 5000
          });
        },
        (error) => {
          let msg = JSON.parse(error._body).Message;
          this.alerts.push({
            type: 'danger',
            msg: msg,
            timeout: 5000
          });
        }
    );
  }

}
