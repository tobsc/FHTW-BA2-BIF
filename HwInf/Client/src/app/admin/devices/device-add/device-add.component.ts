import {Component, OnInit, AfterViewChecked, AfterContentChecked} from '@angular/core';
import {DeviceService} from "../../../shared/services/device.service";
import {CustomFieldsService} from "../../../shared/services/custom-fields.service";
import {DeviceType} from "../../../shared/models/device-type.model";
import {FieldGroup} from "../../../shared/models/fieldgroup.model";
import {Observable} from "rxjs";
import {FormGroup, FormArray, FormBuilder, Validators, NgForm} from "@angular/forms";

@Component({
  selector: 'hwinf-device-add',
  templateUrl: './device-add.component.html',
  styleUrls: ['./device-add.component.scss']
})
export class DeviceAddComponent implements OnInit {

  private myData: string[] = [];
  private form: FormGroup;
  private deviceMeta: FormArray;
  private selectedType;
  private deviceTypes: Observable<DeviceType[]>;
  private fieldGroups: FieldGroup[];

  constructor(
      private deviceService: DeviceService,
      private customFieldsService: CustomFieldsService,
      private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.deviceTypes = this.deviceService.getDeviceTypes();

    this.form = this.fb.group({
      Name: ['', Validators.required],
      InvNum: ['', Validators.required],
      Marke: ['', Validators.required],
      Raum: ['', Validators.required],
      Person: this.initPerson(),
      DeviceMeta: this.fb.array([])
    });
    this.deviceMeta = <FormArray>this.form.controls['DeviceMeta'];
  }

  initPerson() {
    return this.fb.group({
      Uid: ['', Validators.required]
    });
  }

  initDeviceMeta() {
    return this.fb.group({
      Field: ['', Validators.required],
      FieldGroup: ['', Validators.required],
      Value: ['', Validators.required],
    });
  }

  addDeviceMeta() {
    this.deviceMeta.push(this.initDeviceMeta());
  }

  clearDeviceMeta() {

    var myLength = this.deviceMeta.length;
    for (let i = 0; i < myLength; i++) {
      console.log(i+1);
      this.removeDeviceMeta(i);
    }
  }

  removeDeviceMeta(i: number): void {
    this.deviceMeta.removeAt(i);
  }


  onSelectedTypeChange( ) {
    this.clearDeviceMeta();

    this.customFieldsService.getFieldGroupsOfType( this.selectedType )
      .subscribe(
            (data) => {
              this.deviceMeta.reset();
              this.fieldGroups = data;
              for(let i = 0; i<data.length; i++) {
                this.addDeviceMeta();
              }
            }
        );
  }

  onSubmit(form: NgForm) {

    console.log(form.value)
  }

}
