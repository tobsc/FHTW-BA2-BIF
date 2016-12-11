import {Component, OnInit, OnDestroy, Output, EventEmitter, ViewChild} from '@angular/core';
import {DeviceService} from "../device.service";
import {ActivatedRoute} from "@angular/router";
import {Subscription, Observable} from "rxjs";
import {DeviceComponent} from "../device-component.class";
import {NgForm, FormControl} from "@angular/forms";
import {IDictionary} from "../../shared/IDictionary";
import {Dictionary} from "../../shared/Dictionary";
import {URLSearchParams} from "@angular/http";

@Component({
    selector: 'hw-inf-device-filter',
    templateUrl: './device-filter.component.html',
    styleUrls: ['./device-filter.component.scss'],
})
export class DeviceFilterComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    private components: DeviceComponent[];
    private currentType: string;
    private deviceTypes: Observable<string[]>;
    private checkedValues: IDictionary<string[]> = new Dictionary<string[]>();
    @Output() deviceListUpdated = new EventEmitter<URLSearchParams>();
    private term: FormControl = new FormControl();

    constructor(private deviceService: DeviceService, private route: ActivatedRoute) {}

    ngOnInit() {
        this.deviceTypes = this.deviceService.getTypes();
        this.subscription = this.route.params
            .subscribe(
                (params: any) => {
                    this.currentType = params['type'];
                    this.deviceService.getComponents(this.currentType)
                        .subscribe((data: DeviceComponent[]) => {
                            this.components = data;
                            // initialize this.checkedValues with keys and empty arrays
                            for (let c of data) {
                                this.checkedValues.add(c.component.toLowerCase(), []);
                            }
                        });
                }
            );
    }

    /**
     * Updates the list of checked items linked to the checkboxes of the view
     * if an option is checked it will add it to this.checkedValues
     * if an option is unchecked it will delete it from this.checkedValues
     * @param event click event
     */
    private updateCheckedList(event) {
        if(event.target.checked) {
            this.addItem(this.checkedValues.get(event.target.value), event.target.name);
        }
        else {
            this.deleteItem(this.checkedValues.get(event.target.value), event.target.name);
        }
        this.deviceListUpdated.emit(this.searchParams());
    }

    /**
     * Converts this.checkedValues into URLSearchParams
      * @returns {URLSearchParams}
     */
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

    /**
     * pushes given value to given array
     * @param array
     * @param value
     */
    private addItem(array: string[], value: string): void {
        array.push(value);
    }

    /**
     * deletes given value from given array
     * @param array
     * @param value
     */
    private deleteItem(array: string[], value: string): void {
        array.splice(array.indexOf(value), 1);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
