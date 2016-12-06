import {Component, OnInit, OnDestroy} from '@angular/core';
import {Device} from "../device";
import {DeviceService} from "../device.service";
import {Subscription} from "rxjs";
import {ActivatedRoute} from "@angular/router";

@Component({
    selector: 'hw-inf-device-list',
    templateUrl: './device-list.component.html',
    styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    private currentType: string = "";

    private devices: Device[] = [];
    constructor(private deviceService: DeviceService, private route: ActivatedRoute) { }

    ngOnInit() {
        this.subscription = this.route.params
            .subscribe(
                (params: any) => {
                   this.currentType = params['type'];
                }
            );
        this.deviceService.getDevices(this.currentType)
            .subscribe(
                (data: Device[]) => {
                    this.devices = data;
                }
            );
    }


    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
