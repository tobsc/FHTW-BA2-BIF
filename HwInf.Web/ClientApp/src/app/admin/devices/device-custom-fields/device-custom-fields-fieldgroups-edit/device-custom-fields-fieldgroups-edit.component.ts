import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, NgForm } from "@angular/forms";
import { DeviceService } from "../../../../shared/services/device.service";
import { DeviceType } from "../../../../shared/models/device-type.model";
import { CustomFieldsService } from "../../../../shared/services/custom-fields.service";
import { FieldGroup } from "../../../../shared/models/fieldgroup.model";
import { Field } from "../../../../shared/models/field.model";
import { Subscription, Observable } from "rxjs";
import { ActivatedRoute, Params } from "@angular/router";

@Component({
  selector: 'hwinf-device-custom-fields-fieldgroups-edit',
  templateUrl: './device-custom-fields-fieldgroups-edit.component.html',
  styleUrls: ['./device-custom-fields-fieldgroups-edit.component.scss']
})
export class DeviceCustomFieldsFieldgroupsEditComponent implements OnInit {

  

    @Output() deviceTypesListUpdated = new EventEmitter<DeviceType>();
    public form: FormGroup;
    public fields: FormArray;

    public selectableFields: Field[] = [];

    public fieldGroup: FieldGroup;
    public fieldGroups: FieldGroup[] = [];


    public subscription: Subscription;
    public currentFieldGroup: string;
    public startFieldGroupName: string;

    constructor(
        public fb: FormBuilder,
        public deviceService: DeviceService,
        public customFieldsService: CustomFieldsService,
        public route: ActivatedRoute
  ) { }

  get formDataF() { return <FormArray>this.form.controls.Fields; }


    ngOnInit() {
        this.route.params.subscribe((params: Params) => {
            //to set currentType to the actual wanted type
            this.currentFieldGroup = params['slug']
            this.fillFormWithDeviceType(this.currentFieldGroup);
        });



    }

    fillFormWithDeviceType(slug: string) {
        
        //get Fields and Fieldgroup via Slug
        this.customFieldsService.getFieldGroups()
            .map((data) => {
                return data.filter((item) => item.Slug === slug)[0];
            }).subscribe((fGroup: FieldGroup) => {
                this.fieldGroup = fGroup;
                this.startFieldGroupName = this.fieldGroup.Name;
                console.log(this.fieldGroup);
                this.initForm();
            });


        this.form = this.fb.group({
            Name: [this.startFieldGroupName, Validators.required],
            Fields: this.fb.array([])
        });
    }

    initForm() {

        this.form.patchValue({ Name: this.startFieldGroupName });

        this.fields = <FormArray>this.form.controls['Fields'];

        for (let fields of this.fieldGroup.Fields) {
            this.fields.push(this.fb.group({
                Name: [fields.Name, Validators.required]
            }))
        }

        
    }

    initField() {
        this.form.controls['Name'].setValue(this.startFieldGroupName);

        return this.fb.group({
            Name: ['', Validators.required]
        });
    }

    addField() {
        this.fields.push(this.initField());
    }

    clearField() {
        for (var i = 0; i < this.fields.length; i++) {
            this.removeField(i);
        }
    }

    removeField(i: number): void {
        this.fields.removeAt(i);
    }

    onSubmit(form) {

        this.fieldGroup.Name = form.value.Name;
        this.fieldGroup.Fields = form.value.Fields;
        this.customFieldsService.editFieldGroup(this.fieldGroup).subscribe((item: FieldGroup) => {
            console.log(item)
        });
        //this.deviceService.editDeviceType(this.fieldGroup).subscribe(
        //    (next) => {
        //        this.deviceTypesListUpdated.emit(next);
        //        this.form.reset();
        //        this.clearFieldGroup();
        //    },
        //    (error) => console.log(error),
        //    () => console.log('')
        //;

    }

}

