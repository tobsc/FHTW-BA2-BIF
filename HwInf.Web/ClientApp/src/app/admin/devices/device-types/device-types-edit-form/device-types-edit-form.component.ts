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
    }
    constructor(
        private fb: FormBuilder,
        private customFieldsService: CustomFieldsService,
    ) {
        this.customFieldsService.getFieldGroups()
            .subscribe((data) => {
                this.allgroups = data.filter(i => i.Slug !== 'zubehor');
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
        if (!!deviceType && !!deviceType.FieldGroups)
        {
            this.form.get('Name').setValue(deviceType.Name);
            this.form.get('Slug').setValue((deviceType.Slug));
            deviceType.FieldGroups.forEach(i => this.addFieldGroup(i));
        }
      

        
           
        
    }

    initForm() {
        return this.fb.group({
            Name: ['', Validators.required],
            Slug: [''],
         //   IsActive: [false],
            FieldGroups: this.fb.array([])
        });
    }

    clearFieldGroups() {
        for (var i = 0; i < this.fieldgroups.length; i++) {
            this.removeFieldGroup(i);
        }
    }

    addFieldGroup(fieldgroup: FieldGroup = new FieldGroup()) {
        this.fieldgroups.push(this.initFieldGroup(fieldgroup));
    }

    initFieldGroup(fieldgroup  : FieldGroup = new FieldGroup()) {
        return this.fb.group({
            Slug: [fieldgroup.Slug, Validators.required],
            Name: [fieldgroup.Name, ]
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
