import {Component, OnInit, OnDestroy, Output, EventEmitter} from '@angular/core';
import {DeviceService} from "../device.service";
import {ActivatedRoute} from "@angular/router";
import {Subscription, Observable} from "rxjs";
import {DeviceComponent} from "../DeviceComponent.class";

@Component({
    selector: 'hw-inf-device-filter',
    templateUrl: './device-filter.component.html',
    styleUrls: ['./device-filter.component.scss']
})
export class DeviceFilterComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    private components: Observable<DeviceComponent[]>;
    private currentType: string;
    private checkedValues: string[] = [];

    @Output() deviceListUpdated = new EventEmitter<string[]>();

    constructor(private deviceService: DeviceService, private route: ActivatedRoute) {}

    ngOnInit() {
        this.subscription = this.route.params
            .subscribe(
                (params: any) => {
                    this.currentType = params['type'];
                    this.components = this.deviceService.getComponents(this.currentType);
                }
            );
    }

    private updateChecked(event) {

        console.log(event);

        let value: string = event.target.value.toLowerCase();
        if (event.target.checked) {
            this.addItem(value);
        }
        else {
            this.deleteItem(value);
        }
        console.log(this.checkedValues);
        this.deviceListUpdated.emit(this.checkedValues);
    }

    private addItem(value: string) {
        this.checkedValues.push(value);
    }

    private deleteItem(value: string) {
        this.checkedValues.splice(this.checkedValues.indexOf(value), 1);
    }

    private getComponentValues(type: string, component: string) {
        return this.deviceService.getComponentValues(type, component);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
