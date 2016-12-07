import {Component, OnInit, OnDestroy} from '@angular/core';
import {DeviceService} from "../device.service";
import {ActivatedRoute} from "@angular/router";
import {Subscription, Observable} from "rxjs";
import {IDictionary} from "../../shared/IDictionary";
import {Dictionary} from "../../shared/Dictionary";
import {Response} from "@angular/http";

@Component({
    selector: 'hw-inf-device-filter',
    templateUrl: './device-filter.component.html',
    styleUrls: ['./device-filter.component.scss']
})
export class DeviceFilterComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    private components: any[] = [];
    private currentType: string;

    constructor(private deviceService: DeviceService, private route: ActivatedRoute) {}

    ngOnInit() {
        this.subscription = this.route.params
            .subscribe(
                (params: any) => {
                    this.currentType = params['type'];
                    this.deviceService.getComponents(this.currentType).subscribe(
                        (data: string[]) => {
                            this.components = data;
                            console.log(data);
                        }
                    );
                }
            );
    }

    getComponentValues(type: string, component: string) {
        return this.deviceService.getComponentValues(type, component);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
