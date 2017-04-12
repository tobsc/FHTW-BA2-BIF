import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, NgForm } from "@angular/forms";
import { DeviceService } from "../../../../shared/services/device.service";
import { DeviceType } from "../../../../shared/models/device-type.model";
import { CustomFieldsService } from "../../../../shared/services/custom-fields.service";
import { FieldGroup } from "../../../../shared/models/fieldgroup.model";

@Component({
    selector: 'hwinf-device-types-edit',
    templateUrl: './device-types-edit.component.html',
    styleUrls: ['./device-types-edit.component.scss']
})
export class DeviceTypesEditComponent implements OnInit {
    @Output() deviceTypesListUpdated = new EventEmitter<DeviceType>();
    private form: FormGroup;
    private fieldGroups: FormArray;

    private selectableFieldGroups: FieldGroup[] = [];

    private deviceType: DeviceType;

    constructor(
        private fb: FormBuilder,
        private deviceService: DeviceService,
        private customFieldsService: CustomFieldsService
    ) { }

    ngOnInit() {

        this.customFieldsService.getFieldGroups()
            .subscribe((data) => {
                this.selectableFieldGroups = data;
            });


        this.form = this.fb.group({
            Name: ['', Validators.required],
            FieldGroups: this.fb.array([])
        });
        this.fieldGroups = <FormArray>this.form.controls['FieldGroups'];
    }

    initFieldGroup() {
        return this.fb.group({
            Slug: ['', Validators.required]
        });
    }

    addFieldGroup() {
        this.fieldGroups.push(this.initFieldGroup());
    }

    clearFieldGroup() {
        for (var i = 0; i < this.fieldGroups.length; i++) {
            this.removeFieldGroup(i);
        }
    }

    removeFieldGroup(i: number): void {
        this.fieldGroups.removeAt(i);
    }

    onSubmit(form: NgForm) {

        let deviceType: DeviceType = form.value;
        this.deviceService.editDeviceType(deviceType).subscribe(
            (next) => {
                this.deviceTypesListUpdated.emit(next);
                this.form.reset();
                this.clearFieldGroup();
            },
            (error) => console.log(error),
            () => console.log('')
        );

    }

}
