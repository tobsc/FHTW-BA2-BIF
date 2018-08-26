import {Component, OnInit, ViewChild} from '@angular/core';
import {DeviceTypesListComponent} from "./device-types-list/device-types-list.component";
import {DeviceType} from "../../../shared/models/device-type.model";

@Component({
  selector: 'hwinf-device-types',
  templateUrl: './device-types.component.html',
  styleUrls: ['./device-types.component.scss']
})
export class DeviceTypesComponent implements OnInit {

  @ViewChild(DeviceTypesListComponent) public list: DeviceTypesListComponent;

  constructor() { }

  ngOnInit() {
  }

  pushData(item: DeviceType) {
    this.list.pushData(item);
  }

}
