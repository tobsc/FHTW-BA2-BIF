import {Component, OnInit, ViewChild} from '@angular/core';
import {CustomFieldsService} from "../../../shared/services/custom-fields.service";
import {FieldGroup} from "../../../shared/models/fieldgroup.model";
import {FormArray, FormBuilder, FormGroup, NgForm, Validators} from "@angular/forms";
import {Field} from "../../../shared/models/field.model";
import {DeviceAccessoryListComponent} from "./device-accessory-list/device-accessory-list.component";

@Component({
  selector: 'hwinf-device-accessories',
  templateUrl: './device-accessories.component.html',
  styleUrls: ['./device-accessories.component.scss']
})
export class DeviceAccessoriesComponent implements OnInit {

  @ViewChild(DeviceAccessoryListComponent) private list: DeviceAccessoryListComponent;


  constructor() {
  }


  ngOnInit() {

  }

  onAdd(el: any) {
      this.list.pushData(el);
  }
}
