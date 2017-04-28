import {Component, OnInit, EventEmitter, Output, Input} from '@angular/core';
import {CustomFieldsService} from "../../../../shared/services/custom-fields.service";
import {FormBuilder, Validators, FormArray, FormGroup, NgForm} from "@angular/forms";
import {FieldGroup} from "../../../../shared/models/fieldgroup.model";
import {BehaviorSubject, Subject} from "rxjs";

@Component({
  selector: 'hwinf-device-custom-fields-fieldgroups-form',
  templateUrl: './device-custom-fields-fieldgroups-form.component.html',
  styleUrls: ['./device-custom-fields-fieldgroups-form.component.scss']
})
export class DeviceCustomFieldsFieldgroupsFormComponent implements OnInit {

  private fieldGroup$: Subject<FieldGroup> = new BehaviorSubject<FieldGroup>(new FieldGroup());
  private form: FormGroup;
  private fields: FormArray;

  @Output() fieldGroupsListUpdated = new EventEmitter<FieldGroup>();
  @Input() submitButtonName: string;
  @Input()
  private set fieldGroup(fieldGroup) {
    this.fieldGroup$.next(fieldGroup);
  }

  constructor(
      private fb: FormBuilder,
  ) {}

  ngOnInit() {
    this.form = this.initForm();
    this.fields = <FormArray>this.form.controls['Fields'];
    this.fieldGroup$.subscribe((fieldGroup) => {
      this.fillFormWithValues(fieldGroup);
    });
  }

  fillFormWithValues(fieldGroup) {

    console.log("i am called");

    if(!!fieldGroup && !!fieldGroup.Fields)  {
      this.form.get('Name').setValue(fieldGroup.Name);
      this.form.get('Slug').setValue((fieldGroup.Slug));
      this.form.get('IsCountable').setValue(fieldGroup.IsCountable);
      fieldGroup.Fields.forEach(i => this.addField(i.Name));
    }
  }

  initForm() {
    return this.fb.group({
      Name: ['', Validators.required],
      Slug: [''],
      IsCountable: [false],
      Fields: this.fb.array([])
    });
  }

  addField(field: string = '') {
    this.fields.push(this.initField(field));
  }

  clearFields() {
    for (var i = 0; i < this.fields.length; i++) {
      this.removeField(i);
    }
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
    let fieldGroup: FieldGroup = form.value;
    fieldGroup.IsActive = true;
    this.fieldGroupsListUpdated.emit(fieldGroup);
  }
}
