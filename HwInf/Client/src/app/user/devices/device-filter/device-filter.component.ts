import { Component, OnInit } from '@angular/core';
import {CustomFieldsService} from "../../../shared/services/custom-fields.service";
import {FieldGroup} from "../../../shared/models/fieldgroup.model";

@Component({
  selector: 'hwinf-device-filter',
  templateUrl: './device-filter.component.html',
  styleUrls: ['./device-filter.component.scss']
})
export class DeviceFilterComponent implements OnInit {

  private fieldsGroups: FieldGroup[];

  constructor(
      private customFieldsService: CustomFieldsService
  ) { }

  ngOnInit() {


    this.customFieldsService.getFieldGroups().subscribe(
        (data) => this.fieldsGroups = data
    );

  }

}
