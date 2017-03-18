import {Component, OnInit, AfterViewChecked, AfterContentChecked} from '@angular/core';
import {DeviceService} from "../../../shared/services/device.service";
import {CustomFieldsService} from "../../../shared/services/custom-fields.service";
import {DeviceType} from "../../../shared/models/device-type.model";
import {FieldGroup} from "../../../shared/models/fieldgroup.model";
import {Observable} from "rxjs";

@Component({
  selector: 'hwinf-device-add',
  templateUrl: './device-add.component.html',
  styleUrls: ['./device-add.component.scss']
})
export class DeviceAddComponent implements OnInit {

  private selectedType;
  private deviceTypes: Observable<DeviceType[]>;
  private fieldGroups: Observable<FieldGroup[]>;

  constructor(
      private deviceService: DeviceService,
      private customFieldsService: CustomFieldsService
  ) { }

  onSelectedTypeChange( ) {
    this.fieldGroups = this.customFieldsService.getFieldGroupsOfType( this.selectedType );
  }

  ngOnInit() {
    this.deviceTypes = this.deviceService.getDeviceTypes();
  }

}
