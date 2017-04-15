import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, NgForm } from "@angular/forms";
import { DeviceService } from "../../../../shared/services/device.service";
import { DeviceType } from "../../../../shared/models/device-type.model";
import { CustomFieldsService } from "../../../../shared/services/custom-fields.service";
import { FieldGroup } from "../../../../shared/models/fieldgroup.model";
import { Subscription, Observable } from "rxjs";
import { ActivatedRoute, Params } from "@angular/router";

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
    private deviceTypes: DeviceType[] = [];


    private subscription: Subscription;
    private currentType: string;
    private startTypeName: string;

    constructor(
        private fb: FormBuilder,
        private deviceService: DeviceService,
        private customFieldsService: CustomFieldsService,
        private route: ActivatedRoute
    ) { }

    ngOnInit() {
        this.route.params.subscribe((params: Params) => {
            //to set currentType to the actual wanted type
            this.currentType = params['slug']
            this.fillFormWithDeviceType(this.currentType);
        });

        

    }

    fillFormWithDeviceType(slug: string) {
        //get DeviceType via Slug
        this.deviceService.getDeviceTypes().map(devType => {
            return devType.filter(item => item.Slug === this.currentType)[0];
        }).subscribe((devType: DeviceType) => {
            console.log(devType);
            this.startTypeName = devType.Name;
            this.deviceType = devType;
            this.initForm();
        });

        this.customFieldsService.getFieldGroups()
            .subscribe((data) => {
                this.selectableFieldGroups = data;
            });


        this.form = this.fb.group({
            Name: [this.startTypeName, Validators.required],
            FieldGroups: this.fb.array([])
        });
    }

    initForm() {

        this.form.patchValue({ Name: this.startTypeName });

        this.fieldGroups = <FormArray>this.form.controls['FieldGroups'];

        this.customFieldsService.getFieldGroupsOfType(this.currentType)
            .subscribe((data) => {
                for (let fg of data) {
                    this.fieldGroups.push(this.fb.group({
                        Slug: [fg.Slug, Validators.required]
                    }))
                }
            });

    }

    initFieldGroup() {
        this.form.controls['Name'].setValue(this.startTypeName);

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
        
        this.deviceType.Name = form.value.Name;

        //FieldGroup not an array
        this.deviceType.FieldGroups = form.value.FieldGroups;

        this.deviceService.editDeviceType(this.deviceType).subscribe(
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
