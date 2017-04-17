import { Component, OnInit } from '@angular/core';
import {DeviceType} from "../../../../shared/models/device-type.model";
import {DeviceService} from "../../../../shared/services/device.service";
import {CustomFieldsService} from "../../../../shared/services/custom-fields.service";
import {FieldGroup} from "../../../../shared/models/fieldgroup.model";

@Component({
  selector: 'hwinf-device-custom-fields-fieldgroups-list',
  templateUrl: './device-custom-fields-fieldgroups-list.component.html',
  styleUrls: ['./device-custom-fields-fieldgroups-list.component.scss']
})
export class DeviceCustomFieldsFieldgroupsListComponent implements OnInit {

  private fieldGroups: FieldGroup[] = [];

  constructor(private customFieldsService: CustomFieldsService) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    this.customFieldsService.getFieldGroups()
        .subscribe((data) => {
          this.fieldGroups = data;
        });
  }

  pushData(item: FieldGroup) {
    this.fieldGroups.push(item);
  }

  removeDeviceType(index: number) {
      this.fieldGroups.splice(index, 1);
  }

  onDelete(groupSlug: string, index: number) {

      //api for deleting fieldgroup?
      //this.customFieldsService.delete.deleteDeviceType(groupSlug)
      //    .subscribe(
      //    () => { this.removeDeviceType(index) },
      //    (err) => console.log(err)
      //    );
  }

}
