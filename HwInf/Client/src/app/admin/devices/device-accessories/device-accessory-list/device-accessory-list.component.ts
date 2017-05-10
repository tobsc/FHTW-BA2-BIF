import { Component, OnInit } from '@angular/core';
import {Accessory} from "../../../../shared/models/accessory.model";
import {DeviceService} from "../../../../shared/services/device.service";

@Component({
  selector: 'hwinf-device-accessory-list',
  templateUrl: './device-accessory-list.component.html',
  styleUrls: ['./device-accessory-list.component.scss']
})
export class DeviceAccessoryListComponent implements OnInit {

    private rows: any[];

  constructor(private deviceService: DeviceService) { }

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    this.deviceService.getAccessories()
        .subscribe((data) => {
          console.log(data);
            this.rows = data.map(i => ({isCollapsed: true, accessory: i}));
        })
  }


  public pushData(el: Accessory) {
      this.rows.unshift({isCollapsed: true, accessory: el});
  }

  public onSave(row: any, i) {
      this.deviceService.updateAccessory({ AccessoryId: row.accessory.AccessoryId, Name: i.value, Slug: row.accessory.Slug})
          .subscribe(
              (success) => {
                  row.accessory = success;
                  row.isCollapsed = true;
              },
              (err) => console.log(err)
          )
  }

  public onDelete(index: number) {

      this.deviceService.deleteAccessory(this.rows[index].accessory.Slug)
          .subscribe(

              (success) => {
                  this.rows.splice(index,1);
              },
            (error) => console.log(error)

          );


  }

}
