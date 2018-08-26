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

  constructor(
      public deviceService: DeviceService,
  ) { }

  ngOnInit() {

  }

  onSave(deviceType) {
      this.deviceService.addDeviceType(deviceType)
          .subscribe(
          (success) => {
              this.deviceTypesListUpdated.emit(deviceType);
          },
          (error) => console.log(error)
          );
  }

}
