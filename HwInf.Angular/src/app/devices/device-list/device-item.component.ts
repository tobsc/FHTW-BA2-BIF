import {Component, Input} from '@angular/core';
import {Device} from "../Device.class";

@Component({
  selector: 'hw-inf-device-item',
  templateUrl: './device-item.component.html',
  styleUrls: ['./device-item.component.scss']
})
export class DeviceItemComponent {
  @Input() device: Device;
}
