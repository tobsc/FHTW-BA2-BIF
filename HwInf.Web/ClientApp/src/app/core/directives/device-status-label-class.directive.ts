import {Directive, OnInit, Input, HostBinding} from '@angular/core';
import {Status} from "../../shared/models/status.model";

@Directive({
  selector: '[hwinfDeviceStatusLabelClass]'
})
export class DeviceStatusLabelClassDirective implements OnInit {

  @HostBinding('class') public currentClass : string;
  @HostBinding('style.color') public color : string = '#fff';
  @Input() deviceStatus: Status;

  constructor() { }

  ngOnInit(): void {

    switch (this.deviceStatus.StatusId) {
      case 1:
        this.currentClass = 'label label-success';
      break;

      case 2:
        this.currentClass = 'label label-warning';
      break;

      case 3:
        this.currentClass = 'label label-danger';
      break;

      default:
        this.currentClass = 'label label-default';
      break;
    }
  }
}
