import { Component, OnInit } from '@angular/core';
import {DeviceService} from "../device.service";
import {Observable} from "rxjs";

@Component({
  selector: 'hw-inf-device-add',
  templateUrl: './device-add.component.html',
  styleUrls: ['./device-add.component.scss']
})
export class DeviceAddComponent implements OnInit {

  private deviceTypes: Observable<string[]>;

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
    this.deviceTypes = this.deviceService.getTypes();
  }

}
