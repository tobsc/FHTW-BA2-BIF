import {Component, OnInit, AfterViewChecked, AfterContentChecked} from '@angular/core';
import {DeviceService} from "../../../shared/services/device.service";
import {CustomFieldsService} from "../../../shared/services/custom-fields.service";
import {DeviceType} from "../../../shared/models/device-type.model";
import {FieldGroup} from "../../../shared/models/fieldgroup.model";
import {Observable} from "rxjs";
import {FormGroup, FormArray, FormBuilder, Validators, NgForm, Form} from "@angular/forms";
import {Device} from "../../../shared/models/device.model";

@Component({
  selector: 'hwinf-device-add',
  templateUrl: './device-add.component.html',
  styleUrls: ['./device-add.component.scss']
})
export class DeviceAddComponent implements OnInit {

  private form: FormGroup;
  private formFieldGroups: FormArray;
  private invNums: FormArray;
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
      AdditionalInvNums: this.fb.array([]),
      Marke: ['', Validators.required],
      Raum: ['', Validators.required],
      DeviceType: this.initDeviceType(),
      Person: this.initPerson(),
      FieldGroups: this.fb.array([]),
    });
    this.formFieldGroups = <FormArray>this.form.controls['FieldGroups'];
    this.invNums = <FormArray>this.form.controls['AdditionalInvNums'];
  }

  addInvNum() {
    this.invNums.push(this.initInvNum());
  }

  initInvNum(invNum: string = '') {
    return this.fb.group({
      InvNum: invNum
    });
  }

  removeInvNum(index: number) {
    this.invNums.removeAt(index);
  }

  initPerson(uid: string = '') {
    return this.fb.group({
      Uid: [uid, Validators.required]
    });
  }

  initDeviceType( slug: string = '' ) {
    return this.fb.group({
      Slug: [slug, Validators.required]
    })
  }

  clearFieldGroups() {
    let len = this.formFieldGroups.length;
    for ( let i = len; i--;) {
      this.removeFieldGroup(i);
    }
  }

  removeFieldGroup(index: number) {
    this.formFieldGroups.removeAt(index);
  }

  addFieldGroup() {
    this.formFieldGroups.push(this.initFieldGroup());
  }

  initFieldGroup() {
    return this.fb.group({
      Slug: ['', Validators.required],
      Fields: this.fb.array([
          this.initField()
      ])
    });
  }

  initField() {
    return this.fb.group({
      Name: ['', Validators.required],
      Quantity: ['', Validators.required]
    });
  }

  addField(fieldGroupIndex: number) {
    this.getFieldGroup(fieldGroupIndex).push(this.initField());
  }

  removeField(fieldGroupIndex: number, fieldIndex: number) {
    this.getFieldGroup(fieldGroupIndex).removeAt(fieldIndex);
  }

  getFieldGroup(fieldGroupIndex: number): FormArray {
    let fieldGroup: FormGroup = <FormGroup> this.formFieldGroups.at(fieldGroupIndex);
    return <FormArray> fieldGroup.controls['Fields'];
  }


  onSelectedTypeChange( $event ) {
    this.clearFieldGroups();
    this.customFieldsService.getFieldGroupsOfType( $event )
      .subscribe(
            (data) => {
              this.fieldGroups = data;
              for (let i = this.fieldGroups.length; i--;) {
                this.addFieldGroup();
              }
              console.log(this.form);
            }
        );
  }

  initDeviceMeta(fieldGroup: string, field: string,  value: string) {
    return this.fb.group({
      Field: [field, Validators.required],
      FieldGroupSlug: [fieldGroup, Validators.required],
      Value: [value, Validators.required]
    });
  }

  onSubmit(form: NgForm) {

    let finalForm: FormGroup = this.fb.group({
      Name: [form.value.Name, Validators.required],
      InvNum: [form.value.InvNum, Validators.required],
      Marke: [form.value.Marke, Validators.required],
      Raum: [form.value.Raum, Validators.required],
      DeviceType: this.initDeviceType(form.value.DeviceType.Slug),
      Verwalter: this.initPerson(form.value.Person.Uid),
      DeviceMeta: this.fb.array([]),
      AdditionalInvNums: this.fb.array(
          form.value.AdditionalInvNums
      ),
    });

    let deviceMeta: FormArray = <FormArray>finalForm.controls['DeviceMeta'];

    for (let fieldgroup of form.value.FieldGroups) {
      for (let field of fieldgroup.Fields) {
        deviceMeta.push(this.initDeviceMeta(fieldgroup.Slug, field.Name, field.Quantity ))
      }
    }

    console.log(finalForm.value);

   this.deviceService.addNewDevice(<Device>finalForm.value).subscribe(
        (next) => { console.log(next) },
        (error) => { console.log(error) }
    );
  }



}
