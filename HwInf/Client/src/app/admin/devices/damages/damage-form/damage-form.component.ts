import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { CustomFieldsService } from "../../../../shared/services/custom-fields.service";
import { FormBuilder, Validators, FormArray, FormGroup, NgForm } from "@angular/forms";
import { Damage } from "../../../../shared/models/damage.model";
import { DamageStatus } from "../../../../shared/models/damage-status.model";
import { DamageService } from "../../../../shared/services/damage.service";
import { UserService } from "../../../../shared/services/user.service";
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
        this.damageService.getDamageStati().subscribe(i => { this.damageStati = i; });
        this.form = this.initForm();
        this.fillFormWithValues(this.currentDamage);
    }

    fillFormWithValues(damage) {
        if (!!damage && !!damage.Reporter && !!damage.Description) {
            
            if (!!damage.Cause) {
                this.form.get('Cause').get('Uid').setValue(damage.Cause.Uid);
            }
            else {
                this.form.get('Cause').get('Uid').setValue('');
            }
            this.form.get('Date').setValue(damage.Date);
            
            this.form.get('Reporter').get('Uid').setValue(damage.Reporter.Uid);
            this.form.get('Description').setValue(damage.Description);
            this.form.get('Device').get('InvNum').setValue(damage.Device.InvNum);
            this.form.get('DamageStatus').get('Slug').setValue(damage.DamageStatus.Slug);
        }
        else {
            this.userService.getUser().subscribe(i => this.form.get('Reporter').get('Uid').setValue(i.Uid));
        }
    }

    initForm() {
        return this.fb.group({
            Cause: this.initPerson(),
            Date: [{ value: '', disabled: true }],
            Reporter: this.initPerson(),
            Description: ['', Validators.required],
            Device: this.initDevice(),
            DamageStatus: this.initDamageStatus(),
        });
    }

    private initPerson(uid: string = ''): FormGroup {
        return this.fb.group({
            Uid: [uid, Validators.required]
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
        if (this.currentDamage != null) {
            damage.DamageId = this.currentDamage.DamageId;
        }
        this.damageUpdated.emit(damage);
    }

}