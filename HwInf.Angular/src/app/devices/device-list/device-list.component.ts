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
    currentDevice: Device;
    constructor(private deviceService: DeviceService) { }

    ngOnInit() {
        this.deviceService.getDevices()
            .subscribe(
                (data: Device[]) => {
                    this.devices = data;
                    console.log(data);
                }
            );
    }
}
