import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { AdminService } from "../../shared/services/admin.service";
import { Setting } from "../../shared/models/setting.model";


@Component({
  selector: 'hwinf-admin-settings',
  templateUrl: './admin-settings.component.html',
  styleUrls: ['./admin-settings.component.scss']
})
export class AdminSettingsComponent implements OnInit {

    private settings: Setting[] = [];
    private notification: Setting;

    
    constructor(
        private adminService: AdminService
    ) { }

    ngOnInit() {
        this.initSettings();
  }

  // DaterangePicker
  public daterange: any = {};

  // see original project for full list of options
  // can also be setup using the config service to apply to multiple pickers
  public options: any = {
      locale: { format: 'DD.MM' },
      alwaysShowCalendars: false,
      minDate: new Date(),
      maxDate: "31.03.2017" //SEMESTERENDE
  };

  public selectedDate(value: any) {
      this.daterange.start = value.start;
      this.daterange.end = value.end;
  }


  public updateRangePicker() {
      this.options.minDate = "01.01.2017";
  }

  public initSettings() {
      this.getDateSettings();
      this.getMailSettings();
  }

  public getDateSettings() {
      this.adminService.getSetting('ss_start').subscribe(data => this.settings[0] = data);
      this.adminService.getSetting('ss_end').subscribe(data => this.settings[1] = data);
      this.adminService.getSetting('ws_start').subscribe(data => this.settings[2] = data);
      this.adminService.getSetting('ws_end').subscribe(data => this.settings[3] = data);
  }

  public getMailSettings() {
      this.adminService.getSetting('mail_notification_1').subscribe(data => this.notification = data);

  }

}
