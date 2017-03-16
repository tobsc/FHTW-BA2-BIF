import { Component, OnInit } from '@angular/core';
import { Group } from '../../../shared/models/fieldgroup.model';
import { DeviceService } from '../../../shared/services/device.service';
import { Observable } from "rxjs";


@Component({
  selector: 'hwinf-group-manager',
  templateUrl: './group-manager.component.html',
  styleUrls: ['./group-manager.component.scss']
})
export class GroupManagerComponent implements OnInit {
    private fieldgroup: Observable<Group[]> = null;

  constructor() { }

  ngOnInit() {
   //   this.fieldgroup = this.deviceService.getGroups();
  }
  private onSubmit(f) {
   //   this.deviceService.addGroup(f.form.value).subscribe();
  }
 

}
