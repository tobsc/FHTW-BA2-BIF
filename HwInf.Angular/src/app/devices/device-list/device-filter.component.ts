import { Component, OnInit } from '@angular/core';
import {DeviceService} from "../device.service";

@Component({
    selector: 'hw-inf-device-filter',
    templateUrl: './device-filter.component.html',
    styleUrls: ['./device-filter.component.scss']
})
export class DeviceFilterComponent implements OnInit {

    private types: string[] = [];
    private components: string[] = [];

    constructor(private deviceService: DeviceService) {}

    ngOnInit() {
        this.deviceService.getComponents("pc")
            .subscribe(
                (data: string[]) => {
                    this.components = data;
                    console.log(data);
                }
            );

    }

}
