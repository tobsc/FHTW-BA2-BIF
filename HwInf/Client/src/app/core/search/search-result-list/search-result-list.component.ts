import { Component, OnInit } from '@angular/core';
import { DeviceService } from "../../../shared/services/device.service";
import { Device } from "../../../shared/models/device.model";
import { DeviceList } from "../../../shared/models/device-list.model";
import { PubSubSearchService } from "../../../shared/services/pub-sub-search.service";
import { DeviceMeta } from "../../../shared/models/device-meta.model";
import { IDictionary } from "../../../shared/common/dictionary.interface";
import { Dictionary } from "../../../shared/common/dictionary.class";
import { Filter } from "../../../shared/models/filter.model";
import { DeviceType } from "../../../shared/models/device-type.model";
import { CustomFieldsService } from "../../../shared/services/custom-fields.service";
import { FieldGroup } from "../../../shared/models/fieldgroup.model";
import { Router, ActivatedRoute } from "@angular/router";
import { CartService } from "../../../shared/services/cart.service";

@Component({
  selector: 'hwinf-search-result-list',
  templateUrl: './search-result-list.component.html',
  styleUrls: ['./search-result-list.component.scss']
})
export class SearchResultListComponent implements OnInit {

    private devices: Device[] = [];
    private searchText: string;
    private alerts: any = [];

    constructor(private deviceService: DeviceService,
        private pubSubSearchService: PubSubSearchService,
        private route: ActivatedRoute,
        private cartService: CartService
    ) { }

    ngOnInit() {
     /*   this.pubSubSearchService.getSearchText().flatMap(searchText => {
            return this.deviceService.getSearch(searchText)
        }).subscribe(
            data => this.devices = data.Devices); */

           this.route.queryParams
            .flatMap(
                params => { console.log(params);
                     this.searchText = params["searchText"];
                    return this.deviceService.getSearch(this.searchText);
                }).subscribe(
                    data => {
                    this.devices = data.Devices;
                }
            ) 
    }

    public getMetaDataOfFieldGroup(slug: string, metaData: DeviceMeta[]): DeviceMeta[] {
        return metaData.filter((i) => i.FieldGroupSlug === slug);
    }

    public addItem(device: Device): void {
        this.cartService.addItem(device);
        this.alerts.push({
            type: 'success',
            msg: `Das Gerät ${device.Name} wurde zum Warenkorb hinzufefügt.`,
            timeout: 5000
        });
    }
    
}
