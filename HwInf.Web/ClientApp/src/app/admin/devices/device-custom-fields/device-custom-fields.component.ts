import {Component, OnInit, ViewChild} from '@angular/core';
import {DeviceCustomFieldsFieldgroupsListComponent} from "./device-custom-fields-fieldgroups-list/device-custom-fields-fieldgroups-list.component";
import {FieldGroup} from "../../../shared/models/fieldgroup.model";

@Component({
  selector: 'hwinf-device-custom-fields',
  templateUrl: './device-custom-fields.component.html',
  styleUrls: ['./device-custom-fields.component.scss']
})
export class DeviceCustomFieldsComponent implements OnInit {

  @ViewChild(DeviceCustomFieldsFieldgroupsListComponent) public list: DeviceCustomFieldsFieldgroupsListComponent;

  constructor() { }

  ngOnInit() {
  }

  pushData(item: FieldGroup) {
    this.list.pushData(item);
  }

}
