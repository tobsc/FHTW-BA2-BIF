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
    private rows: any[];

  constructor(private customFieldsService: CustomFieldsService) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    this.customFieldsService.getFieldGroups()
        .subscribe((data) => {
          this.fieldGroups = data;
            this.rows = data.map(i => ({isCollapsed: true, fieldGroup: i}));
        });
  }

  pushData(fieldGroup: FieldGroup) {
    this.fieldGroups.unshift(fieldGroup);
      this.rows.unshift({isCollapsed:true, fieldGroup: fieldGroup});
  }

  removeDeviceType(index: number) {
      this.fieldGroups.splice(index, 1);
      this.rows.splice(index, 1);
  }

  onDelete(groupSlug: string, index: number) {
      this.customFieldsService.deleteFieldGroup(groupSlug)
          .subscribe(
          () => { this.removeDeviceType(index) },
          (err) => console.log(err)
          );
  }

  onSave(i, fieldGroup) {
      this.customFieldsService.editFieldGroup(fieldGroup)
          .subscribe(
              (success) => {
                  console.log(success);
                  this.rows[i].fieldGroup = success;
                  this.rows[i].isCollapsed = true;
              },
              (error) => console.log(error)
          );
  }

}
