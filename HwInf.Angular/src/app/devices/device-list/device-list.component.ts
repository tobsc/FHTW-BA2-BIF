import {Component, OnInit, OnDestroy} from '@angular/core';
import {Device} from "../Device.class";
import {DeviceService} from "../device.service";
import {Subscription, Observable} from "rxjs";
import {ActivatedRoute} from "@angular/router";
import {URLSearchParams} from "@angular/http";

@Component({
    selector: 'hw-inf-device-list',
    templateUrl: './device-list.component.html',
    styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    private currentType: string;
    private devices: Observable<Device[]>;

    constructor(private deviceService: DeviceService, private route: ActivatedRoute) { }

    ngOnInit() {
        this.subscription = this.route.params
            .subscribe(
                (params: any) => {
                   this.currentType = params['type'];
                   this.devices = this.deviceService.getDevices(this.currentType);
                }
            );
    }

    private updateList(params: URLSearchParams) {
        this.devices = this.deviceService.getDevices(this.currentType, params);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
