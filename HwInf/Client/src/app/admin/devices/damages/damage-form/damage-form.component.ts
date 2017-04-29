import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { CustomFieldsService } from "../../../../shared/services/custom-fields.service";
import { FormBuilder, Validators, FormArray, FormGroup, NgForm } from "@angular/forms";
import { Damage } from "../../../../shared/models/damage.model";
import { DamageStatus } from "../../../../shared/models/damage-status.model";
import { DamageService } from "../../../../shared/services/damage.service";
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
    @Input() currentDamage: Damage;

    constructor(
        private fb: FormBuilder,
        private damageService: DamageService
    ) { }



    ngOnInit() {
        console.log(this.currentDamage);
        this.damageService.getDamageStati().subscribe(i => { this.damageStati = i; });
        this.form = this.initForm();
        this.fillFormWithValues(this.currentDamage);
    }

    fillFormWithValues(damage) {

        if (!!damage && !!damage.Reporter) {
            if (!!damage.Cause) {
                this.form.get('Cause').setValue(damage.Cause.Uid);
            }
            else {
                this.form.get('Cause').setValue('');
            }
            this.form.get('Reporter').setValue(damage.Reporter.Uid);
            this.form.get('Description').setValue(damage.Description);
            this.form.get('Device').setValue(damage.Device.InvNum);
            this.form.get('DamageStatus').setValue(damage.DamageStatus.Name);
        }
    }

    initForm() {
        return this.fb.group({
            Cause: ['', Validators.required],
            Reporter: [''],
            Description: [''],
            Device: [''],
            DamageStatus: ['']
        });
    }
    
    public resetForm(): void {
        this.form.reset();
    }

}
