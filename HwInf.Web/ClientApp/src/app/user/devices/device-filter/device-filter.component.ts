import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import {CustomFieldsService} from "../../../shared/services/custom-fields.service";
import {FieldGroup} from "../../../shared/models/fieldgroup.model";
import {DeviceType} from "../../../shared/models/device-type.model";
import {Subscription} from "rxjs";
import {ActivatedRoute} from "@angular/router";
import {FormBuilder, FormGroup, FormArray} from "@angular/forms";
import {Filter} from "../../../shared/models/filter.model";
import {Field} from "../../../shared/models/field.model";
import {DeviceMeta} from "../../../shared/models/device-meta.model";

@Component({
  selector: 'hwinf-device-filter',
  templateUrl: './device-filter.component.html',
  styleUrls: ['./device-filter.component.scss']
})
export class DeviceFilterComponent implements OnInit {
  @Input() inputFilter: Filter;
  @Output() outputFilter: EventEmitter<DeviceMeta[]> = new EventEmitter<DeviceMeta[]>();
  @Input() deviceType: DeviceType;
  public fieldGroups: FieldGroup[];
  public sub: Subscription;
  public isCollapsedArr: boolean[];
  public form: FormGroup;
  public fieldGroupsFormArray: FormArray;
  public count = 0;

  constructor(
      public customFieldsService: CustomFieldsService,
      public route: ActivatedRoute,
      public fb: FormBuilder
  ) { }

  ngOnInit() {
    this.form = this.initForm();
    this.sub = this.route.params
          .map( i => i['type'])
          .flatMap( i => this.customFieldsService.getFieldGroupsOfTypeForFilter(i))
          .subscribe(
              (data: FieldGroup[]) => {
                this.fieldGroups = data.filter(i => i.Fields.length > 0);
                this.isCollapsedArr = [];
                this.fieldGroups.forEach(() => this.isCollapsedArr.push(true));
                this.form = this.initForm();
                this.fieldGroupsFormArray = <FormArray>this.form.controls['FieldGroup'];

                this.fieldGroups.forEach((item, index) => {
                  this.addFieldGroup(item);
                });
              }
          );
  }

  initForm(): FormGroup {
    return this.fb.group({
      FieldGroup:  this.fb.array([])
    });
  }

  public getCount() {
    return this.count++;
  }

  addDeviceMeta(index: number): void{
  }

  addFieldGroup(fieldGroup: FieldGroup): void {
    this.fieldGroupsFormArray.push(this.initFieldGroup(fieldGroup));
  }

  initFieldGroup(fieldGroup: FieldGroup): FormGroup {
    let foo =  this.fb.group({
      MetaQuery: this.fb.array([])
    });

    let metaquery = <FormArray> foo.controls['MetaQuery'];

    fieldGroup.Fields.forEach(() => metaquery.push(this.initDeviceMeta()));

    return foo;
  }

  initDeviceMeta(): FormGroup {
    return this.fb.group({
      isChecked: [false],
      FieldGroupSlug: [],
      FieldSlug: [],
      Value: []
    });
  }

  onCollapse(index: number) {
    this.isCollapsedArr[index] = !this.isCollapsedArr[index];
  }


  onSubmit(form: FormGroup, ev: Event) {

    const flatten = arr => arr.reduce(
     (acc, val) => acc.concat(
     Array.isArray(val) ? flatten(val) : val
     ),[]);

    let arr: any[]= form.value.FieldGroup;
    arr = flatten(arr.map(i => i.MetaQuery))
        .filter(i => i.isChecked)
        .map(i => <DeviceMeta>({
              FieldGroupSlug: i.FieldGroupSlug,
              FieldSlug: i.FieldSlug,
              Value: i.Value
            })
        );
    this.outputFilter.emit(arr);
  }

  mappedForm(form: FormGroup): DeviceMeta[] {

    return form.value.MetaQuery
        .filter(i => i.isChecked)
        .map(i => <DeviceMeta>({
            FieldGroupSlug: i.FieldGroupSlug,
            FieldSlug: i.FieldSlug,
            Value: i.Value
          })
        );
  }


}
