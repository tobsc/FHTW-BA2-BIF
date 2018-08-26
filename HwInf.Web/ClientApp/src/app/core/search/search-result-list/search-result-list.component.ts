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

    public devices: Device[] = [];
    public searchText: string;
    public alerts: any = [];
    public page: number = 1;
    public limit: number = 20;
    public totalItems: number = 0;
   

    constructor(public deviceService: DeviceService,
        public pubSubSearchService: PubSubSearchService,
        public route: ActivatedRoute,
        public cartService: CartService
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
                    let offset = (this.page - 1) * this.limit;
                    return this.deviceService.getSearch(this.searchText, this.limit, offset);
                }).subscribe(
               data => {
                        this.totalItems = data.TotalItems;
                        this.devices = data.Devices;
                        this.page = 1;
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

    public onLoadMore() {
        if (this.totalItems > this.devices.length) {
            this.page++;
            let offset = (this.page - 1) * this.limit;
            this.deviceService.getSearch(this.searchText, this.limit, offset).subscribe(
                data => {
                    if (data.Devices.length > 0) {
                        this.devices = this.devices.concat(data.Devices);
                    }
                }
            )
        }    
    }
    
}
