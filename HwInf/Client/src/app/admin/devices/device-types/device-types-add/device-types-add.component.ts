import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import {FormGroup, FormBuilder, Validators, FormArray, NgForm} from "@angular/forms";
import {DeviceService} from "../../../../shared/services/device.service";
import {DeviceType} from "../../../../shared/models/device-type.model";
import {CustomFieldsService} from "../../../../shared/services/custom-fields.service";
import {FieldGroup} from "../../../../shared/models/fieldgroup.model";

@Component({
  selector: 'hwinf-device-types-add',
  templateUrl: './device-types-add.component.html',
  styleUrls: ['./device-types-add.component.scss']
})
export class DeviceTypesAddComponent implements OnInit {
  @Output() deviceTypesListUpdated = new EventEmitter<DeviceType>();
  private form: FormGroup;
  private fieldGroups: FormArray;

  private selectableFieldGroups: FieldGroup[] = [];

  constructor(
      private fb: FormBuilder,
      private deviceService: DeviceService,
      private customFieldsService: CustomFieldsService,
      private route: Router
  ) { }

  ngOnInit() {

   this.customFieldsService.getFieldGroups()
        .subscribe( (data) => {
          this.selectableFieldGroups = data;
        });


    this.form = this.fb.group({
      Name: ['', Validators.required],
      FieldGroups: this.fb.array([])
    });
    this.fieldGroups = <FormArray>this.form.controls['FieldGroups'];
  }

  initFieldGroup() {
    return this.fb.group({
      Slug: ['', Validators.required]
    });
  }

  addFieldGroup() {
    this.fieldGroups.push(this.initFieldGroup());
  }

  clearFieldGroup() {
    for (var i = 0; i < this.fieldGroups.length; i++) {
      this.removeFieldGroup(i);
    }
  }

  removeFieldGroup(i: number): void {
    this.fieldGroups.removeAt(i);
  }

  onSubmit(form : NgForm) {

      let deviceType: DeviceType = form.value;

      console.log(deviceType);
    this.deviceService.addDeviceType(deviceType).subscribe(
        (next) => {
          this.deviceTypesListUpdated.emit(next);
          this.form.reset();
          this.clearFieldGroup();
        },
        (error) => console.log(error),
        () => console.log('')
    );

      //BAD HACK
    location.reload();
  }

}
