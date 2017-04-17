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
export class DeviceCustomFieldsFieldgroupsAddComponent implements OnInit {
  @Output() fieldGroupsListUpdated = new EventEmitter<FieldGroup>();

  private form: FormGroup;
  private fields: FormArray;

  constructor(
      private fb: FormBuilder,
      private customFieldsService: CustomFieldsService
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      Name: ['', Validators.required],
      Fields: this.fb.array([])
    });
    this.fields = <FormArray>this.form.controls['Fields'];
  }

  initField() {
    return this.fb.group({
      Name: ['', Validators.required]
    });
  }

  addField() {
    this.fields.push(this.initField());
  }

  clearFields() {
    for (var i = 0; i < this.fields.length; i++) {
      this.removeField(i);
    }
  }

  removeField(i: number) {
    this.fields.removeAt(i);
  }
  onSubmit(form: NgForm) {
    let fieldGroup: FieldGroup = form.value;
    fieldGroup.IsActive = true;
    this.customFieldsService.addFieldGroup(fieldGroup).subscribe(
        (next) => {
          this.fieldGroupsListUpdated.emit(next);
          this.form.reset();
          this.clearFields();
        },
        (err) => console.log(err)
    );
  }

}
