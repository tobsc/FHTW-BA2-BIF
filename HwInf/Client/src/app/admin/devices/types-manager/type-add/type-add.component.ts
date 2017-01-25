import { Component, OnInit } from '@angular/core';
import { DeviceService } from "../../../../devices/shared/device.service";
import { Observable } from "rxjs";


@Component({
  selector: 'hw-inf-type-add',
  templateUrl: './type-add.component.html',
  styleUrls: ['./type-add.component.scss']
})
export class TypeAddComponent implements OnInit {

   
    private num: number = 0;
    private numarray: number[] = [];
    constructor(
        private deviceService: DeviceService

    ) {
    }

    ngOnInit() {
        this.addNewRow();

    }
    private addNewRow() {
        this.num++;
        this.numarray.push(this.num);
    }

    private addDeviceType(ngForm) {

    }
    private onSubmit(f) {
        console.log(f.form.value);
        this.deviceService.addDeviceType(f.form.value).subscribe();
    }

}
