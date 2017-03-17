import { Component, OnInit } from '@angular/core';
import {NgForm, FormGroup, FormBuilder, Validators, FormArray} from "@angular/forms";
import {FieldGroup} from "../../../../shared/models/fieldgroup.model";

@Component({
  selector: 'hwinf-device-groups-add',
  templateUrl: './device-groups-add.component.html',
  styleUrls: ['./device-groups-add.component.scss']
})
export class DeviceGroupsAddComponent implements OnInit {

  private myForm: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.myForm = this.fb.group({
      Name: ['', [Validators.required]],
      Fields: this.fb.array([
        this.initFields(),
      ])
    });
  }


  initFields() {
    // initialize our address
    return this.fb.group({
      Name: ['', Validators.required],
      Slug: ['']
    });
  }

  addField() {
    // add address to the list
    const control = <FormArray>this.myForm.controls['Fields'];
    control.push(this.initFields());
  }

  removeField(i: number) {
    // remove address from the list
    const control = <FormArray>this.myForm.controls['Fields'];
    control.removeAt(i);
  }

  onSubmit(fieldgroup: NgForm) {
    console.log (fieldgroup);
  }

}
