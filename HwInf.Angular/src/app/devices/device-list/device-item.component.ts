import {Component, Input} from '@angular/core';
import {Device} from "../device.class";

@Component({
  selector: 'hw-inf-device-item',
  templateUrl: './device-item.component.html',
  styleUrls: ['./device-item.component.scss']
})
export class DeviceItemComponent {
  @Input() device: Device;

  /**
   * @param statusId status of device
   * @returns the appropriate bootstrap 3 class according to the status id
   */
  getStatusLabelClass(statusId: number): string {
    switch(statusId) {
      case 1: return 'label-success';
      case 2: return 'label-warning';
      case 3: return 'label-danger';
      default: return 'label-default';
    }
  }
}
