import {Component, OnInit, ViewChild} from '@angular/core';
import {CustomFieldsService} from "../../../shared/services/custom-fields.service";
import {FieldGroup} from "../../../shared/models/fieldgroup.model";
import {FormArray, FormBuilder, FormGroup, NgForm, Validators} from "@angular/forms";
import {Field} from "../../../shared/models/field.model";

@Component({
  selector: 'hwinf-device-accessories',
  templateUrl: './device-accessories.component.html',
  styleUrls: ['./device-accessories.component.scss']
})
export class DeviceAccessoriesComponent implements OnInit {

  constructor(
      private customFieldsService: CustomFieldsService,
  ) { }

  @ViewChild('pop')
  private toolTip;
  private fieldGroup: FieldGroup = new FieldGroup();
  public alerts: any = [];
  private tags: string[] = [];

  ngOnInit() {
    this.customFieldsService.getFieldGroups()
        .subscribe((data) => {
          this.fieldGroup = data.filter(i => i.Slug === 'zubehor')[0];
          this.tags = this.fieldGroup.Fields.map(i => i.Name);
        });
  }
  onAdd(el: any) {
    if(this.tags.indexOf(el.value) < 0 && !this.isNullOrWhiteSpace(el.value)) {
      this.tags.unshift(el.value);
      el.value = '';
    } else {
      this.toolTip.show();
    }
  }

  bla() {
    this.toolTip.hide();
  }

  onDelete(i: number) {
    this.tags.splice(i,1);
  }

  isNullOrWhiteSpace(str) {
    return (!str || str.length === 0 || /^\s*$/.test(str))
  }

  onSubmit() {
    this.fieldGroup.Fields = <Field[]>this.tags.map(i => ({ Name: i}));
    this.customFieldsService.editFieldGroup(this.fieldGroup).subscribe(
        (succes) => {
          this.alerts.push({
            type: 'success',
            msg: `Zubehör erfolgreich gespeichert!`,
            timeout: 5000
          })
        },
        (error) => {
          this.alerts.push({
            type: 'danger',
            msg: `Fehler beim speichern!`,
            timeout: 5000
          })
        }
    );
  }
}
