import { Component, OnInit } from '@angular/core';
import { DeviceService } from "../../../devices/shared/device.service";
import { Observable } from "rxjs";


@Component({
  selector: 'hw-inf-types-manager',
  templateUrl: './types-manager.component.html',
  styleUrls: ['./types-manager.component.scss']
})
export class TypesManagerComponent implements OnInit {

    private types: Observable<string[]> = null;
   
    constructor(
        private deviceService: DeviceService
        
    ) { 
  }

    ngOnInit() {
        this.types = this.deviceService.getTypes();
        
  }
   

}
