import {Component, OnInit, ViewChild} from '@angular/core';
import {DeviceAccessoryListComponent} from "./device-accessory-list/device-accessory-list.component";

@Component({
  selector: 'hwinf-device-accessories',
  templateUrl: './device-accessories.component.html',
  styleUrls: ['./device-accessories.component.scss']
})
export class DeviceAccessoriesComponent implements OnInit {

  @ViewChild(DeviceAccessoryListComponent) public list: DeviceAccessoryListComponent;


  constructor() {
  }


  ngOnInit() {

  }

  onAdd(el: any) {
      this.list.pushData(el);
  }
}
