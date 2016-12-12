import {Component, OnInit, AfterViewInit, Input, ViewChild, OnDestroy} from '@angular/core';
import {DeviceService} from "../device.service";
import {Observable, Subscription} from "rxjs";
import {DeviceComponent} from "../device-component.class";
import {NgForm} from "@angular/forms";
import {Device} from "../device.class";
import {ModalComponent} from "../../common/modal/modal.component";
import {Router} from "@angular/router";

@Component({
  selector: 'hw-inf-device-add',
  templateUrl: './device-add.component.html',
  styleUrls: ['./device-add.component.scss']
})
export class DeviceAddComponent implements OnInit, OnDestroy {

  @ViewChild('confirmModal') private readonly confirmModal: ModalComponent;
  @ViewChild('errorModal') private readonly errorModal: ModalComponent;
  private deviceTypes: string[] = [];
  private deviceComponents: Observable<DeviceComponent[]>;
  private selectedType: number = 1;
  private data;
  private subscription: Subscription;

  constructor(private deviceService: DeviceService, private router: Router) { }


  ngOnInit() {
    this.deviceService.getTypes()
      .subscribe((data: string[]) => {
        this.deviceTypes = data;
        this.deviceComponents = this.deviceService.getComponentsAndValues(this.deviceTypes[this.selectedType-1]);
      });

  }

  private onSubmit(form: NgForm) {
    this.confirmModal.show();
  }

  private addDevice(form: NgForm) {
    let tmpDevice: Device = form.form.value;
    tmpDevice.StatusId = '1';
    this.subscription = this.deviceService.addDevice(tmpDevice)
      .subscribe(
        (data) => {
          this.data = data;
          console.log(data);
          this.router.navigate(['/devices']);
        },
        (error) => {
          this.errorModal.show(error);
        });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  private onSelectedTypeChange(value): void {
    this.deviceComponents = this.deviceService.getComponentsAndValues(this.deviceTypes[value-1]);
  }
}
