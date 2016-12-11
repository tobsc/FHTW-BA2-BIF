import {Component, OnInit, AfterViewInit, Input} from '@angular/core';
import {DeviceService} from "../device.service";
import {Observable} from "rxjs";
import {DeviceComponent} from "../device-component.class";
import {NgForm} from "@angular/forms";
import {Device} from "../device.class";
import {error} from "util";

@Component({
  selector: 'hw-inf-device-add',
  templateUrl: './device-add.component.html',
  styleUrls: ['./device-add.component.scss']
})
export class DeviceAddComponent implements OnInit {
  private deviceTypes: string[] = [];
  private deviceComponents: Observable<DeviceComponent[]>;
  private selectedType: number = 1;

  private data;
  private error;

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
    this.deviceService.getTypes()
      .subscribe((data: string[]) => {
        this.deviceTypes = data;
        this.deviceComponents = this.deviceService.getComponents(this.deviceTypes[this.selectedType-1]);
      });

  }

  private onSubmit(form: NgForm) {
    let tmpDevice: Device = form.form.value;
    tmpDevice.StatusId = '1';
    console.log(JSON.stringify(tmpDevice));
    this.deviceService.addDevice(tmpDevice)
      .subscribe(
        (data) => {
          this.data = data;
          console.log(data)
        },
        (error) => {
          this.error = error;
          console.log(error);
        });
  }

  private onSelectedTypeChange(value): void {
    this.deviceComponents = this.deviceService.getComponents(this.deviceTypes[value-1]);
  }
}
