import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {FieldGroup} from "../../../../shared/models/fieldgroup.model";
import {FormGroup, FormArray, FormBuilder, Validators, NgForm} from "@angular/forms";
import {DeviceService} from "../../../../shared/services/device.service";
import {CustomFieldsService} from "../../../../shared/services/custom-fields.service";

@Component({
  selector: 'hwinf-device-custom-fields-fieldgroups-add',
  templateUrl: './device-custom-fields-fieldgroups-add.component.html',
  styleUrls: ['./device-custom-fields-fieldgroups-add.component.scss']
})
export class DeviceCustomFieldsFieldgroupsAddComponent {
  @Output() fieldGroupsListUpdated = new EventEmitter<FieldGroup>();

  constructor(
      public customFieldsService: CustomFieldsService
  ) {}


  addFieldGroup(fieldGroup, formComponent) {

    this.customFieldsService.addFieldGroup(fieldGroup).subscribe(
        (next) => {
          this.emit(next);
          formComponent.resetForm();
        },
        (error) => console.log(error)
    );
  }


  emit(event) {
    this.fieldGroupsListUpdated.emit(event);
  }
}
