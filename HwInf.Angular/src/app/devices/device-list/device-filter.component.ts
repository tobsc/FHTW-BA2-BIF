import { Component, OnInit } from '@angular/core';
import {DeviceService} from "../device.service";

@Component({
  selector: 'hw-inf-device-filter',
  templateUrl: './device-filter.component.html',
  styleUrls: ['./device-filter.component.css']
})
export class DeviceFilterComponent implements OnInit {

  private types: string[] = [];

  constructor(private deviceService: DeviceService) {}

  ngOnInit() {
    this.deviceService.getTypes()
        .subscribe(
            (data: string[]) => {
              this.types = data;
              console.log(data);
            }
        );
  }

}
