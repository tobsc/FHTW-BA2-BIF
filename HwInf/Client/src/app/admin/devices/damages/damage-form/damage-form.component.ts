import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { CustomFieldsService } from "../../../../shared/services/custom-fields.service";
import { FormBuilder, Validators, FormArray, FormGroup, NgForm } from "@angular/forms";
import { Damage } from "../../../../shared/models/damage.model";
import { DamageStatus } from "../../../../shared/models/damage-status.model";
import { DamageService } from "../../../../shared/services/damage.service";
import { UserService } from "../../../../shared/services/user.service";
import { User } from "../../../../shared/models/user.model";
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
    private selectedUser: User;
    private selectedString: string;
    private ownUser: User;
    private ownString: string;

    @Output() damageUpdated = new EventEmitter<Damage>();
    @Input() submitButtonName: string;
    @Input() currentDamage: Damage=null;
    @Input() showDate: boolean;

    constructor(
        private fb: FormBuilder,
        private damageService: DamageService,
        private userService: UserService
    ) { }



    ngOnInit() {
        this.form = this.initForm();

        this.damageService.getDamageStati().subscribe(i => { this.damageStati = i; });
        this.userService.getUsers().subscribe(i => {
            this.users = i;

            //dictionary with custom strings as keys
            this.users.forEach((user, index) => {
                this.userDic[this.objectFormatter(user)] = user;
                this.stringForDic[index] = this.objectFormatter(user);
            });
        });
        this.userService.getUser().subscribe(i => {
            this.ownUser = i;
            
            this.fillFormWithValues(this.currentDamage);
        });
        
    }

    fillFormWithValues(damage) {
        if (!!damage && !!damage.Reporter && !!damage.Description) {
            if (!!damage.Cause) {
                this.form.get('Cause').setValue(this.objectFormatter(damage.Cause));
            }
            else {
                this.form.get('Cause').setValue('');
            }
            this.form.get('Date').setValue(damage.Date);
            
            this.form.get('Reporter').setValue(this.objectFormatter(damage.Reporter));
            this.form.get('Description').setValue(damage.Description);
            this.form.get('Device').get('InvNum').setValue(damage.Device.InvNum);
            this.form.get('DamageStatus').get('Slug').setValue(damage.DamageStatus.Slug);
        }
        else {
            this.form.get('Reporter').setValue(this.objectFormatter(this.ownUser));
        }
    }

    initForm() {
        return this.fb.group({
            Cause: [''],
            Date: [{ value: '', disabled: true }],
            Reporter: [''],
            Description: ['', Validators.required],
            Device: this.initDevice(),
            DamageStatus: this.initDamageStatus(),
        });
    }

    private initDamageStatus(slug: string = ''): FormGroup {
        return this.fb.group({
            Slug: [slug, Validators.required]
        })
    }

    private initDevice(InvNum: string = ''): FormGroup {
        return this.fb.group({
            InvNum: [InvNum, Validators.required]
        });
    }
    
    public resetForm(): void {
        this.form.reset();
    }

    onSubmit(form: NgForm) {
        let damage: Damage = form.value;
        damage.Cause = this.userDic[form.value.Cause];
        damage.Reporter = this.userDic[form.value.Reporter];
        if (this.currentDamage != null) {
            damage.DamageId = this.currentDamage.DamageId;
        }
        this.damageUpdated.emit(damage);
    }

    objectFormatter(data: any): string {
        return "(" + data.Uid + ") " + data.LastName + " " + data.Name;
    }
}
