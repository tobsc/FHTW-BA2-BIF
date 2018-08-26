import { Component, OnInit } from '@angular/core';
import { NgForm } from "@angular/forms";
import { DeviceService } from "../../shared/services/device.service";
import { PubSubSearchService } from "../../shared/services/pub-sub-search.service";
import { Router } from "@angular/router";

@Component({
  selector: 'hwinf-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {

    constructor(public deviceService: DeviceService,
        public pubSubSearchService: PubSubSearchService,
        public router: Router
    ) { }

    ngOnInit() {

  }

  public executeSearch(searchText: any) 
  {
      if (searchText.length > 0) {
          this.router.navigate(["/suche"], { queryParams: { searchText: searchText }});
         // this.router.navigate(["/suche"]);
         // this.pubSubSearchService.publishSearchText(searchText); 
      }
              
            
  }
}
