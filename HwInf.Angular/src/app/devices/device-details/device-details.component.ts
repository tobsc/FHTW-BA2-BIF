import {Component, OnInit, OnDestroy} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {DeviceService} from "../device.service";
import {Subscription} from "rxjs";
import {Device} from "../Device.class";

@Component({
    selector: 'hw-inf-device-details',
    templateUrl: './device-details.component.html',
    styleUrls: ['./device-details.component.scss']
})
export class DeviceDetailsComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    private currentDevice: Device;

    constructor(private deviceService: DeviceService,
                private route: ActivatedRoute) { }

    ngOnInit() {
        this.subscription = this.route.params
            .subscribe(
                (params: any) => {
                    let deviceId: number = params['id'];
                    this.deviceService.getDevice(deviceId).subscribe(
                        (data: Device[]) => {
                            this.currentDevice = data[0];
                            console.log(this.currentDevice);
                        }
                    );;
                }
            );
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
