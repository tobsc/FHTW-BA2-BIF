import { Component, OnInit } from '@angular/core';
import {DeviceService} from "./shared/device.service";
import {Observable} from "rxjs";

@Component({
    selector: 'hw-inf-devices-start',
    templateUrl: './devices-start.component.html',
    styleUrls: ['./devices-start.component.scss']
})
export class DevicesStartComponent implements OnInit {

    private types: Observable<string[]>;

    constructor(private deviceService: DeviceService) { }

    ngOnInit() {
        this.types = this.deviceService.getTypes();
    }

}
