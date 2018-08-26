
import {Component, OnInit} from "@angular/core";
import {DeviceService} from "../../../shared/services/device.service";
import {Device} from "../../../shared/models/device.model";
import { ErrorHandlerService } from "../../../shared/services/error-handler.service";
import { Router } from "@angular/router";

@Component({
  selector: 'hwinf-device-add',
  templateUrl: './device-add.component.html',
  styleUrls: ['./device-add.component.scss']
})
export class DeviceAddComponent implements OnInit {

  public alerts: any = [];
  constructor(
      public deviceService: DeviceService,
      public router:Router
  ) { }

  ngOnInit() {
  }

  onSubmit(device: Device) {

    this.deviceService.addNewDevice(device).subscribe(
        (next) => {
            this.router.navigate(['/admin/geraete/verwalten'], { skipLocationChange: true, queryParams: { 'page': 1, 'orderby': 'createdate' } });

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
