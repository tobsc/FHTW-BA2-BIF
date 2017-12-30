import {Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges} from '@angular/core';
import {DeviceService} from "../../../shared/services/device.service";
import {CustomFieldsService} from "../../../shared/services/custom-fields.service";
import {ActivatedRoute} from "@angular/router";
import {FormBuilder, FormGroup, FormArray, Validators, NgForm} from "@angular/forms";
import {UserService} from "../../../shared/services/user.service";
import {Subscription, Observable} from "rxjs";
import {DeviceType} from "../../../shared/models/device-type.model";
import {FieldGroup} from "../../../shared/models/fieldgroup.model";
import {User} from "../../../shared/models/user.model";
import {Device} from "../../../shared/models/device.model";
import {DeviceMeta} from "../../../shared/models/device-meta.model";
import {Status} from "../../../shared/models/status.model";


@Component({
  selector: 'hwinf-device-form',
  templateUrl: './device-form.component.html',
  styleUrls: ['./device-form.component.scss']
})
export class DeviceFormComponent implements OnInit {

  @Input() buttonLabel: string;
  @Input() feature: string;
  @Output() submitForm = new EventEmitter<Device>();
  private form: FormGroup;
  private formFieldGroups: FormArray;
  private invNums: FormArray;
  private deviceTypes: Observable<DeviceType[]>;
  private deviceStatuses: Observable<Status[]>;
  private fieldGroups: FieldGroup[];  
  private currentDevice: Device;
  private formDeviceType: FormGroup;
  private currentTypeSlug: string;

  private owners: User[];
  private ownerDic: { [search: string]: User; } = {};
  private stringForDic: string[] = [];

  //private mask = ['([A-Za-z0-9]{2}\+){2}([A-Za-z0-9]{4})'];
  private r = /[A-Za-z0-9]/;
  private mask = [this.r, this.r, '+', this.r, this.r, '+', this.r, this.r, this.r, this.r];

    //for noInv devices
  private invNumFlag: boolean = true;

  constructor(
      private deviceService: DeviceService,
      private customFieldsService: CustomFieldsService,
      private userService: UserService,
      private fb: FormBuilder,
      private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.init();
  }

  init() {
    this.deviceTypes = this.deviceService.getDeviceTypes();
    this.deviceStatuses = this.deviceService.getDeviceStatuses();

    this.userService.getOwners()
        .subscribe(i => {
            this.owners = i;

            this.owners.forEach((owner, index) => {
                this.ownerDic[this.userFormatter(owner)] = owner;
                this.stringForDic[index] = this.userFormatter(owner);
            });
        });
          

    

    this.form = this.initForm();
    this.formDeviceType = <FormGroup>this.form.controls['DeviceType'];
    this.formFieldGroups = <FormArray>this.form.controls['FieldGroups'];
    this.invNums = <FormArray>this.form.controls['AdditionalInvNums'];


    if ( this.feature === 'add' ) return;

    this.route.params
        .map(params => params['id'])
        .flatMap(id => this.deviceService.getDeviceById(id))
        .subscribe(
            (device:Device) => {
              this.currentDevice = device;
              this.fillFormWithDeviceData(device);
            },
            (err) => { console.log(err); }
        );
  }

  /**
   * @param slug
   * @param metaData
   * @returns {DeviceMeta[]} array of device meta data of given slug
   */
  private getMetaDataOfFieldGroup(slug: string, metaData: DeviceMeta[]): DeviceMeta[] {
    return metaData.filter((i) => i.FieldGroupSlug === slug);
  }

  /**
   * fills the meta data of the form with given device data
   * @param device
   */
  private fillFormWIthMetaData(device: Device) {
      
    device.FieldGroups
        .forEach( (fieldgroup, index) => {
          this.getFieldGroups(index).removeAt(0);
          this.getMetaDataOfFieldGroup(fieldgroup.Slug, device.DeviceMeta)
              .forEach((field) => {
                this.getFieldGroups(index).push(this.initField(field.Field, +field.Value));
              });
        });
  }

  /**
   * fills the form with given Device data
   * @param device
   */
  private fillFormWithDeviceData(device: Device) {
    this.form.get('Name').setValue(device.Name);
    this.form.get('Marke').setValue(device.Marke);
    this.form.get('Raum').setValue(device.Raum);
    this.form.get('Notiz').setValue(device.Notiz);
    this.form.get('Person').setValue(this.userFormatter(device.Verwalter));
    this.form.get('DeviceType').setValue({Slug: device.DeviceType.Slug });

    if (this.feature === 'edit') {
      this.form.get('InvNum').setValue(device.InvNum);
      this.form.get('Status').setValue(device.Status);
    }
  }


  /**
   * Initializes a the main form
   * @returns {FormGroup}
   */
  private initForm(device: Device = null): FormGroup {
    return this.fb.group({
      Name: ['', Validators.required],
      InvNumFlag: [''],
      InvNum: ['', Validators.required],
      Quantity: [''],
      AdditionalInvNums: this.fb.array([]),
      Marke: ['', Validators.required],
      Raum: ['', Validators.required],
      DeviceType: this.initDeviceType(),
      Person: [ '', Validators.required ],
      FieldGroups: this.fb.array([]),
      Status: this.initStatus(),
      Notiz: ['']
    });
  }

  /**
   * Add an additional inventory number
   */
  public addInvNum(): void {
    this.invNums.push(this.initInvNum());
  }

  /**
   * Intializes a new form group
   * @param invNum
   * @returns {FormGroup}
   */
  private initInvNum(invNum: string = ''): FormGroup {
    return this.fb.group({
      InvNum: invNum
    });
  }

  /**
   * remove inventory number field at index i
   * @param index
   */
  public removeInvNum(index: number): void {
    this.invNums.removeAt(index);
  }

  private initStatus(id: number = 1): FormGroup {
    return this.fb.group({
      StatusId: [id, Validators.required],
      Description: ['']
    });
  }

  /**
   * Initializes a new DeviceType form group
   * @param slug
   * @returns {FormGroup}
   */
  private initDeviceType( slug: string = '' ): FormGroup {
    return this.fb.group({
      Slug: [slug, Validators.required]
    });
  }

  /**
   * Remove all FieldGroups
   */
  private clearFieldGroups(): void {
    let len = this.formFieldGroups.length;
    for ( let i = len; i--;) {
      this.removeFieldGroup(i);
    }
  }

  /**
   * Remove FieldGroup at given index i
   * @param index
   */
  public removeFieldGroup(index: number): void {
    this.formFieldGroups.removeAt(index);
  }

  /**
   * Add new FieldGroup
   */
  public addFieldGroup(): void {
    this.formFieldGroups.push(this.initFieldGroup());
  }

  /**
   * Initializes a new FieldGroup with 1 Field
   * @returns {FormGroup}
   */
  private initFieldGroup(): FormGroup {
    return this.fb.group({
      Slug: ['', Validators.required],
      Fields: this.fb.array([this.initField()])
    });
  }

  /**
   * Initializes a Field form group
   * @returns {FormGroup}
   */
  public initField(name: string = '', quantity :number = 1): FormGroup {
    return this.fb.group({
      Name: name,
      Quantity: quantity
    });
  }

  /**
   * Add new Field to FieldGroup with given index
   * @param fieldGroupIndex
   */
  public addField(fieldGroupIndex: number): void {
    this.getFieldGroups(fieldGroupIndex).push(this.initField());
  }

  /**
   * Remove Field from at index i from FieldGroup with index j
   * @param fieldGroupIndex
   * @param fieldIndex
   */
  removeField(fieldGroupIndex: number, fieldIndex: number) {
    this.getFieldGroups(fieldGroupIndex).removeAt(fieldIndex);
  }

  /**
   * @param fieldGroupIndex
   * @returns {FormArray}  of FieldGroups
   */
  private getFieldGroups(fieldGroupIndex: number): FormArray {
    let fieldGroup: FormGroup = <FormGroup> this.formFieldGroups.at(fieldGroupIndex);
    return <FormArray> fieldGroup.controls['Fields'];
  }

  /**
   * if the selected DeviceType is changed,
   * generate FieldGroups and Field according to the FieldGroups of the DeviceType
   * @param $event
   */
  public onSelectedTypeChange( $event ): void {
    this.clearFieldGroups();
    this.currentTypeSlug = $event;
    this.customFieldsService.getFieldGroupsOfType( $event )
        .subscribe(
            (data) => {
              this.fieldGroups = data;
              this.fieldGroups.forEach(() => this.addFieldGroup());

              if (this.currentDevice && this.currentDevice.DeviceType.Slug === this.currentTypeSlug) {
                this.fillFormWIthMetaData(this.currentDevice);
              }
            }
        );
  }

  /**
   * Initializes DeviceMeta field group with given params
   * @param fieldGroup  slug of the FieldGroup
   * @param field       Name of the Field
   * @param value       Quantity of the field
   * @returns {FormGroup}
   */
  private initDeviceMeta(fieldGroup: string, field: string,  value: string): FormGroup {
    return this.fb.group({
      Field: [field, Validators.required],
      FieldGroupSlug: [fieldGroup, Validators.required],
      Value: [value, Validators.required]
    });
  }


  public onSubmit(form: NgForm, event: Event): void {
      event.preventDefault();
      
      this.mappedForm(form);
      this.submitForm.emit(this.mappedForm(form));
  }

  /**
   * Maps the form to a Device for the API call
   * @param form
   * @returns {Device} mapped form of type Device
   */
  private mappedForm(form: NgForm): Device {
    let resultForm: FormGroup = this.fb.group({
      DeviceId: (this.currentDevice) ? this.currentDevice.DeviceId : -1, // -1 ModelState fix hack
      Name: [form.value.Name, Validators.required],
      InvNum: ['', Validators.required],
      Quantity: [''],
      Marke: [form.value.Marke, Validators.required],
      Raum: [form.value.Raum, Validators.required],
      DeviceType: this.initDeviceType(form.value.DeviceType.Slug),
      Verwalter: this.ownerDic[form.value.Person],
      DeviceMeta: this.fb.array([]),
      AdditionalInvNums: this.fb.array(
          form.value.AdditionalInvNums
      ),
      Status: [form.value.Status],
      Notiz: [form.value.Notiz]
    });

    if (this.form.value.InvNumFlag == true) {
        resultForm.controls['InvNum'].setValue(this.form.value.InvNum);
    }
    else {
        resultForm.controls['Quantity'].setValue(this.form.value.Quantity);
    }

    let deviceMeta: FormArray = <FormArray>resultForm.controls['DeviceMeta'];

    form.value.FieldGroups
        .forEach(i => {
          i.Fields.forEach(j => {
            if (j.Name) {
              deviceMeta.push(this.initDeviceMeta(i.Slug, j.Name, j.Quantity))
            }
          })
        });

      //for debugging purposes
    console.log(<Device>resultForm.value);
    return <Device>resultForm.value;
  }
  
  userFormatter(data: any): string {
      return /*"(" + data.Uid + ") " +*/ data.LastName + " " + data.Name;
  }

  hasInvNum(): boolean {
      return this.invNumFlag;
  }

  changeInvNumFlag(): void {
      if (this.invNumFlag) {
          this.form.controls['InvNum'].setValidators(null);
          this.form.controls['InvNum'].setValue("");
          this.form.controls['Quantity'].setValidators([Validators.required]);
      }
      else {
          this.form.controls['InvNum'].setValidators([Validators.required]);
          this.form.controls['Quantity'].setValidators(null);
          this.form.controls['Quantity'].setValue("");
      }

      this.invNumFlag = !this.invNumFlag;
  }
}
