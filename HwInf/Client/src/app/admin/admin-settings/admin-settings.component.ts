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

    private settings: Setting[]=this.adminService.getSettings();
    private notification: Setting;


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
      startDate: this.settings[0].Value+".2017",
      endDate: this.settings[1].Value+".2017",
  };

  public optionsWS: any = {
      autoUpdateInput: true,
      locale: { format: this.DATE_FORMAT },
      alwaysShowCalendars: false,
      startDate: this.settings[2].Value + ".2017",
      endDate: this.settings[3].Value + ".2017",
  };

  public selectedDate(value: any) {
      this.daterange.start = value.start;
      this.daterange.end = value.end;
  }

}
