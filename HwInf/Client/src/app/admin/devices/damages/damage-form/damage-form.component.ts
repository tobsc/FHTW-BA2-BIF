import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { ActivatedRoute, Router, Params } from "@angular/router";
import { CustomFieldsService } from "../../../../shared/services/custom-fields.service";
import { FormBuilder, Validators, FormArray, FormGroup, NgForm } from "@angular/forms";
import { Damage } from "../../../../shared/models/damage.model";
import { DamageStatus } from "../../../../shared/models/damage-status.model";
import { DamageService } from "../../../../shared/services/damage.service";
import { UserService } from "../../../../shared/services/user.service";
import { User } from "../../../../shared/models/user.model";
import { DeviceService } from "../../../../shared/services/device.service";
import { Device } from "../../../../shared/models/device.model";
import { DeviceList } from "../../../../shared/models/device-list.model";
import { BehaviorSubject, Subject } from "rxjs";

@Component({
  selector: 'hwinf-damage-form',
  templateUrl: './damage-form.component.html',
  styleUrls: ['./damage-form.component.scss']
})
export class DamageFormComponent implements OnInit {

    private form: FormGroup;
    private damageInfo: FormArray;
    private damageStati: DamageStatus[];

    //for auto-complete tofuix [object Object]-bug
    private users: User[];
    private userDic: { [search: string]: User; } = {};
    private stringForDic: string[] = [];
    private ownUser: User;
    private ownString: string;

    private startDevice: Device;
    private devices: Device[];
    private deviceDic: { [search: string]: Device; } = {};
    private stringForDevDic: string[] = [];
    
    @Output() damageUpdated = new EventEmitter<Damage>();
    @Input() submitButtonName: string;
    @Input() currentDamage: Damage;
    @Input() showDate: boolean;

    constructor(
        private fb: FormBuilder,
        private damageService: DamageService,
        private userService: UserService,
        private deviceService: DeviceService,
        private route: ActivatedRoute,
    ) { }

    ngOnInit() {
        this.route.params.subscribe((params: Params) => {
            let invNum = params['invnum'];
            if (invNum) {
                this.deviceService.getDevice(invNum).subscribe(i => this.startDevice = i);
            }
        });
        this.form = this.initForm();

        this.damageService.getDamageStati().subscribe(i => { this.damageStati = i; });
        this.userService.getUsers().subscribe(i => {
            this.users = i;

            //dictionary with custom strings as keys
            this.users.forEach((user, index) => {
                this.userDic[this.userFormatter(user)] = user;
                this.stringForDic[index] = this.userFormatter(user);
            });
        });
        this.deviceService.getDevices().subscribe(i => {
            this.devices = i.Devices;

            this.devices.forEach((device, index) => {
                this.deviceDic[this.deviceFormatter(device)] = device;
                this.stringForDevDic[index] = this.deviceFormatter(device);
            })
        })
        this.userService.getUser().subscribe(i => {
            this.ownUser = i;
            
            this.fillFormWithValues(this.currentDamage);
        });
        
    }

    fillFormWithValues(damage) {
        if (!!damage && !!damage.Reporter && !!damage.Description) {
            if (!!damage.Cause) {
                this.form.get('Cause').setValue(this.userFormatter(damage.Cause));
            }
            else {
                this.form.get('Cause').setValue('');
            }
            this.form.get('Date').setValue(damage.Date);
            
            this.form.get('Reporter').setValue(this.userFormatter(damage.Reporter));
            this.form.get('Description').setValue(damage.Description);
            this.form.get('Device').setValue(this.deviceFormatter(damage.Device));
            this.form.get('DamageStatus').get('Slug').setValue(damage.DamageStatus.Slug);
        }
        else {
            if (!!this.startDevice) {
                this.form.get('Device').setValue(this.deviceFormatter(this.startDevice));
            }
            this.form.get('Reporter').setValue(this.userFormatter(this.ownUser));
        }
    }

    initForm() {
        if (this.showDate) {
            return this.fb.group({
                Cause: [{ value: ''}],
                Date: [{ value: '', disabled: true }],
                Reporter: [{ value: '', disabled: true }],
                Description: ['', Validators.required],
                Device: [{ value: '', disabled: true }],
                DamageStatus: this.initDamageStatus(),
            });
        }
        else {
            return this.fb.group({
                Cause: [''],
                Date: [{ value: '', disabled: true }],
                Reporter: [{value: '', disabled: true}],
                Description: ['', Validators.required],
                Device: [''],
                DamageStatus: this.initDamageStatus(),
            });
        }
    }

    private initDamageStatus(slug: string = ''): FormGroup {
        return this.fb.group({
            Slug: [slug, Validators.required]
        })
    }
        
    public resetForm(): void {
        this.form.reset();
        this.form.get('Reporter').setValue(this.userFormatter(this.ownUser));
        if (!!this.startDevice) {
            this.form.get('Device').setValue(this.deviceFormatter(this.startDevice));
        }
    }

    onSubmit(form: NgForm) {
        let damage: Damage = form.value;
        damage.Cause = this.userDic[form.value.Cause];
        damage.Reporter = this.userDic[form.value.Reporter];
        damage.Device = this.deviceDic[form.value.Device];
        if (this.currentDamage != null) {
            damage.DamageId = this.currentDamage.DamageId;
        }
        console.log(damage);
        this.damageUpdated.emit(damage);
    }

    userFormatter(data: any): string {
        return "(" + data.Uid + ") " + data.LastName + " " + data.Name;
    }

    deviceFormatter(data: any): string {
        return data.InvNum + ": " + data.Marke + " " + data.Name;
    }
}
