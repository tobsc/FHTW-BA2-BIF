import {Component, OnInit, OnDestroy, Output, EventEmitter} from '@angular/core';
import {DeviceService} from "../device.service";
import {ActivatedRoute} from "@angular/router";
import {Subscription, Observable} from "rxjs";
import {DeviceComponent} from "../DeviceComponent.class";
import {NgForm} from "@angular/forms";
import {IDictionary} from "../../shared/IDictionary";
import {Dictionary} from "../../shared/Dictionary";
import {URLSearchParams} from "@angular/http";

@Component({
    selector: 'hw-inf-device-filter',
    templateUrl: './device-filter.component.html',
    styleUrls: ['./device-filter.component.scss']
})
export class DeviceFilterComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    private components: DeviceComponent[];
    private currentType: string;
    private checkedValues: IDictionary<string[]> = new Dictionary<string[]>();
    @Output() deviceListUpdated = new EventEmitter<URLSearchParams>();

    constructor(private deviceService: DeviceService, private route: ActivatedRoute) {}

    ngOnInit() {
        this.subscription = this.route.params
            .subscribe(
                (params: any) => {
                    this.currentType = params['type'];
                    this.deviceService.getComponents(this.currentType)
                        .subscribe((data: DeviceComponent[]) => {
                            this.components = data;
                            for (let c of data) {
                                this.checkedValues.add(c.component.toLowerCase(), []);
                            }
                        });
                }
            );
    }

    private updateList(event) {
        if(event.target.checked) {
            this.addItem(this.checkedValues.get(event.target.value), event.target.name);
        }
        else {
            this.deleteItem(this.checkedValues.get(event.target.value), event.target.name);
        }

        console.log(this.searchParams().toString());
        this.deviceListUpdated.emit(this.searchParams());
    }

    private searchParams(): URLSearchParams {
        let result = new URLSearchParams();
        for (let key of this.checkedValues.keys()) {
            let arr: string[] = this.checkedValues.get(key);
            for (let value of arr) {
                result.append(key, value);
            }
        }
        return result;
    }

    private addItem(array: string[], value: string): void {
        array.push(value);
    }

    private deleteItem(array: string[], value: string): void {
        array.splice(array.indexOf(value), 1);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
