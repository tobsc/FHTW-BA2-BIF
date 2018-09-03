import {Component, OnInit, ViewChild} from '@angular/core';
import {User} from "../../../shared/models/user.model";
import {DeviceTypesListComponent} from "../../devices/device-types/device-types-list/device-types-list.component";
import {RemoveAdminComponent} from "./remove-admin/remove-admin.component";

@Component({
  selector: 'hwinf-edit-admins',
  templateUrl: './edit-admins.component.html',
  styleUrls: ['./edit-admins.component.scss']
})
export class EditAdminsComponent implements OnInit {

  @ViewChild(RemoveAdminComponent) private list: RemoveAdminComponent;

  constructor() { }

  ngOnInit() {
  }

  pushData(item: User) {
    this.list.pushData(item);
  }

}
