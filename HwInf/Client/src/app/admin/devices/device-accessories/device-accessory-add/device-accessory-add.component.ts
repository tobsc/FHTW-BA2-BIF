import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {DeviceService} from "../../../../shared/services/device.service";
import {Accessory} from "../../../../shared/models/accessory.model";

@Component({
  selector: 'hwinf-device-accessory-add',
  templateUrl: './device-accessory-add.component.html',
  styleUrls: ['./device-accessory-add.component.scss']
})
export class DeviceAccessoryAddComponent implements OnInit {

  @Output() accessoriesUpdated = new EventEmitter<Accessory>();

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
  }

  onSubmit(f) {
    this.deviceService.addAccessory(f).subscribe(
        (success) => {
          console.log(success);
          this.accessoriesUpdated.emit(success);
        },
        (err) => console.log(err)

  );
  }

}
