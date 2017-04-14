import {Component, OnInit, Input} from '@angular/core';
import {CustomFieldsService} from "../../../shared/services/custom-fields.service";
import {FieldGroup} from "../../../shared/models/fieldgroup.model";
import {DeviceType} from "../../../shared/models/device-type.model";
import {Observable} from "rxjs";

@Component({
  selector: 'hwinf-device-filter',
  templateUrl: './device-filter.component.html',
  styleUrls: ['./device-filter.component.scss']
})
export class DeviceFilterComponent implements OnInit {

  @Input() deviceType: DeviceType;
  @Input() customFields: Observable<FieldGroup[]>;
  private fieldGroups: FieldGroup[];

  constructor(
      private customFieldsService: CustomFieldsService
  ) { }

  ngOnInit() {

      this.customFields.subscribe( (data) => this.fieldGroups = data );
  }

}
