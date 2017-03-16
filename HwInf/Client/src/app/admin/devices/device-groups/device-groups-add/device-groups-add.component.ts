import { Component, OnInit } from '@angular/core';
import { NgForm } from "@angular/forms";

@Component({
  selector: 'hwinf-device-groups-add',
  templateUrl: './device-groups-add.component.html',
  styleUrls: ['./device-groups-add.component.scss']
})
export class DeviceGroupsAddComponent implements OnInit {
  private fields: number[] = [];
  private count = 0;

  constructor() { }

  ngOnInit() {
  }

  public onAddRow(): void {
    this.fields.push(this.count++);
  }

  public onDeleteRow(index) {
    this.fields.splice(index, 1);
  }
  private onSubmit(f: NgForm)
  {
      console.log(f.form.value);   
  }
}
