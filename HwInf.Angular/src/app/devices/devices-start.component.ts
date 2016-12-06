import { Component, OnInit } from '@angular/core';
import {DeviceService} from "./device.service";

@Component({
    selector: 'hw-inf-devices-start',
    templateUrl: './devices-start.component.html',
    styleUrls: ['./devices-start.component.scss']
})
export class DevicesStartComponent implements OnInit {

    private types: string[] = [];

    constructor(private deviceService: DeviceService) { }

    ngOnInit() {
        this.deviceService.getTypes()
            .subscribe(
                (data: string[]) => {
                    this.types = data;
                }
            );
    }

}
