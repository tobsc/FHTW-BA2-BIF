import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { DeviceService } from "../../../../shared/services/device.service";
import { FormBuilder, Validators, FormArray, FormGroup, NgForm } from "@angular/forms";
import { DeviceType } from "../../../../shared/models/device-type.model";
import { BehaviorSubject, Subject } from "rxjs";

@Component({
  selector: 'hwinf-device-types-edit-form',
  templateUrl: './device-types-edit-form.component.html',
  styleUrls: ['./device-types-edit-form.component.scss']
})
export class DeviceTypesEditFormComponent implements OnInit {

    private deviceType$: BehaviorSubject<DeviceType> = new BehaviorSubject<DeviceType>(new DeviceType());
    private form: FormGroup;
    private fields: FormArray;

    @Output() deviceTypesListUpdated = new EventEmitter<DeviceType>();
    @Input() submitButtonName: string;
    @Input()
    private set deviceType(deviceType) {
        this.deviceType$.next(deviceType);
        console.log(deviceType);
    }
    constructor(
        private fb: FormBuilder,
    ) { }

    ngOnInit() {
        this.form = this.initForm();
        this.fields = <FormArray>this.form.controls['FieldGroups'];
        this.deviceType$.subscribe((deviceType) => {
            this.fillFormWithValues(deviceType);
        });
    }

    fillFormWithValues(deviceType) {

        console.log("i am called");
        console.log(deviceType);

        if (!!deviceType && !!deviceType.FieldGroups) {
            this.form.get('Name').setValue(deviceType.Name);
            this.form.get('Slug').setValue((deviceType.Slug));
            deviceType.FieldGroups.forEach(i => this.addField(i.Name));
        }
    }

    initForm() {
        return this.fb.group({
            Name: ['', Validators.required],
            Slug: [''],
            IsActive: [false],
            FieldGroups: this.fb.array([])
        });
    }

    clearFields() {
        for (var i = 0; i < this.fields.length; i++) {
            this.removeField(i);
        }
    }

    addField(field: string = '') {
        this.fields.push(this.initField(field));
    }

    initField(field: string = '') {
        return this.fb.group({
            Name: [field, Validators.required]
        });
    }

    removeField(i: number) {
        this.fields.removeAt(i);
    }

    public resetForm(): void {
        this.form.reset();
        this.clearFields();
    }

    onSubmit(form: NgForm) {
        let deviceType: DeviceType = form.value;
        deviceType.FieldGroups.IsActive = true;
        this.deviceTypesListUpdated.emit(deviceType);
    }

}
