import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'hwinf-device-types-add',
  templateUrl: './device-types-add.component.html',
  styleUrls: ['./device-types-add.component.scss']
})
export class DeviceTypesAddComponent implements OnInit {
  private fieldgroups: number[] = [];
  private count = 0;

  constructor() { }

  ngOnInit() {
  }

  public onAddRow(): void {
    this.fieldgroups.push(this.count++);
  }

  public onDeleteRow(index) {
    this.fieldgroups.splice(index, 1);
  }
}
