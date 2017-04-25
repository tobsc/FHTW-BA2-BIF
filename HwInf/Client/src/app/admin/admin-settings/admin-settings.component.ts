import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { AdminService } from "../../shared/services/admin.service";
import { Setting } from "../../shared/models/setting.model";
var moment = require('moment');


@Component({
  selector: 'hwinf-admin-settings',
  templateUrl: './admin-settings.component.html',
  styleUrls: ['./admin-settings.component.scss']
})
export class AdminSettingsComponent implements OnInit {
        
    private now: Date = new Date();
    private year: number = this.now.getFullYear();
    private ssStart: Setting = {
        Key: "ss_start",
        Value: sessionStorage.getItem("ss_start")
    };
    private ssEnd: Setting = {
        Key: "ss_end",
        Value: sessionStorage.getItem("ss_end")
    };
    private wsStart: Setting = {
        Key: "ws_start",
        Value: sessionStorage.getItem("ws_start")
    };
    private wsEnd: Setting = {
        Key: "ws_end",
        Value: sessionStorage.getItem("ws_end")
    };
    private notification: Setting = {
        Key: "mail_notification_1",
        Value: sessionStorage.getItem("mail_notification_1")
    };

    private setList: Setting[] = [this.ssStart, this.ssEnd, this.wsStart, this.wsEnd, this.notification];



    // DaterangePicker
    public daterange: any = {};
    private readonly DATE_FORMAT: string = 'DD.MM.YYYY';
    
    constructor(
        private adminService: AdminService
    ) {}

    ngOnInit() {
  }


  // see original project for full list of options
  // can also be setup using the config service to apply to multiple pickers
  public optionsSS: any = {
      autoUpdateInput: true,
      locale: { format: this.DATE_FORMAT },
      alwaysShowCalendars: false,   
      startDate: this.ssStart.Value+"."+this.year,
      endDate: this.ssEnd.Value + "." + this.year,
  };

  public optionsWS: any = {
      autoUpdateInput: true,
      locale: { format: this.DATE_FORMAT },
      alwaysShowCalendars: false,
      startDate: this.wsStart.Value + "." + this.year,
      endDate: this.wsEnd.Value + "." + this.year,
  };

  public selectedSSDate(value: any) {
      this.ssStart.Value = value.start.format("DD.MM");
      this.ssEnd.Value = value.end.format("DD.MM");
  }

  public selectedWSDate(value: any) {
      this.wsStart.Value = value.start.format("DD.MM");
      this.wsEnd.Value = value.end.format("DD.MM");
  }
        
  updateSettings() {
      this.adminService.updateSettings(this.setList).subscribe(item => { console.log(item) },(error) => console.log(error));
  }


}
