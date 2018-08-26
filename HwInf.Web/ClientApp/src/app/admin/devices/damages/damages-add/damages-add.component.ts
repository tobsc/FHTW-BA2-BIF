import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Damage } from "../../../../shared/models/damage.model";
import { FormGroup, FormArray, FormBuilder, Validators, NgForm } from "@angular/forms";
import { DamageService } from "../../../../shared/services/damage.service";

@Component({
  selector: 'hwinf-damages-add',
  templateUrl: './damages-add.component.html',
  styleUrls: ['./damages-add.component.scss']
})
export class DamagesAddComponent {

    @Output() damageUpdated = new EventEmitter<Damage>();

    constructor(
        public damageService: DamageService
    ) { }


    addDamage(damage, formComponent) {

        this.damageService.createDamage(damage).subscribe(
            (next) => {
                this.emit(next);
                formComponent.resetForm();
            },
            (error) => console.log(error)
        );
    }


    emit(event) {
        this.damageUpdated.emit(event);
    }
}
