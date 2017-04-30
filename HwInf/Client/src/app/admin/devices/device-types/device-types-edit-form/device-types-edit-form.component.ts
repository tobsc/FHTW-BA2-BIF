import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { DeviceService } from "../../../../shared/services/device.service";
import { CustomFieldsService } from "../../../../shared/services/custom-fields.service";
import { FormBuilder, Validators, FormArray, FormGroup, NgForm } from "@angular/forms";
import { DeviceType } from "../../../../shared/models/device-type.model";
import { FieldGroup } from "../../../../shared/models/fieldgroup.model";
import { BehaviorSubject, Subject, Observable } from "rxjs";

@Component({
  selector: 'hwinf-device-types-edit-form',
  templateUrl: './device-types-edit-form.component.html',
  styleUrls: ['./device-types-edit-form.component.scss']
})
export class DeviceTypesEditFormComponent implements OnInit {

    private deviceType$: BehaviorSubject<DeviceType> = new BehaviorSubject<DeviceType>(new DeviceType());
    private form: FormGroup;
    private fieldgroups: FormArray;
    private allgroups: FieldGroup[] = [];

    @Output() deviceTypesListUpdated = new EventEmitter<DeviceType>();
    @Input() submitButtonName: string;
    @Input()
    private set deviceType(deviceType) {
        this.deviceType$.next(deviceType);
        // console.log(deviceType);
    }
    constructor(
        private fb: FormBuilder,
        private customFieldsService: CustomFieldsService,
    ) {
        this.customFieldsService.getFieldGroups()
            .subscribe((data) => {
                this.allgroups = data;
                console.log(data);
            });
    }

    ngOnInit() {
        
        this.form = this.initForm();
        this.fieldgroups = <FormArray>this.form.controls['FieldGroups'];
        this.deviceType$.subscribe((deviceType) => {
            this.fillFormWithValues(deviceType);
        });
       
    
   
     
    }

    fillFormWithValues(deviceType) {

        console.log("i am called");
        console.log(deviceType);

        //console.log(deviceType.Name);
            this.form.get('Name').setValue(deviceType.Name);
          //  console.log(deviceType.Name);
            this.form.get('Slug').setValue((deviceType.Slug));
            deviceType.FieldGroups.forEach(i => this.addFieldGroup(i.Name));
         //   deviceType.FieldGroups.forEach(i => this.form.get('FieldGroups').setValue({ Name: deviceType.Name }))
           
        
    }

    initForm() {
        return this.fb.group({
            Name: ['', Validators.required],
            Slug: [''],
            IsActive: [false],
            FieldGroups: this.fb.array([])
        });
    }

    clearFieldGroups() {
        for (var i = 0; i < this.fieldgroups.length; i++) {
            this.removeFieldGroup(i);
        }
    }

    addFieldGroup(field: string = '') {
        console.log(field);
        this.fieldgroups.push(this.initFieldGroup(field));
    }

    initFieldGroup(field: string = '') {
       // console.log(field);
        return this.fb.group({
            Name: [field, Validators.required]
        });
    }

    removeFieldGroup(i: number) {
        this.fieldgroups.removeAt(i);
    }

    public resetForm(): void {
        this.form.reset();
        this.clearFieldGroups();
    }

    onSubmit(form: NgForm) {
        let deviceType: DeviceType = form.value;
        deviceType.FieldGroups.IsActive = true;
        this.deviceTypesListUpdated.emit(deviceType);
    }

}
