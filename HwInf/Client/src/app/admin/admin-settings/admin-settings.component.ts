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
    private newOrder: Setting = {
        Key: "new_order_mail",
        Value: sessionStorage.getItem("new_order_mail")
    };
    private mailAccept_above: Setting = {
        Key: "accept_mail_above",
        Value: sessionStorage.getItem("accept_mail_above")
    }

    private mailAccept_below: Setting = {
        Key: "accept_mail_below",
        Value: sessionStorage.getItem("accept_mail_below")
    }

    private mailDecline_above: Setting = {
        Key: "decline_mail_above",
        Value: sessionStorage.getItem("decline_mail_above")
    }

    private mailDecline_below: Setting = {
        Key: "decline_mail_below",
        Value: sessionStorage.getItem("decline_mail_below")
    }

    private mailReminder: Setting = {
        Key: "reminder_mail",
        Value: sessionStorage.getItem("reminder_mail")
    }

    private daysBefore: Setting = {
        Key: "days_before_reminder",
        Value: sessionStorage.getItem("days_before_reminder")
    }

    private setList: Setting[] = [this.ssStart, this.ssEnd, this.wsStart, this.wsEnd, this.newOrder, this.mailAccept_above, this.mailAccept_below, this.mailDecline_above, this.mailDecline_below, this.mailReminder, this.daysBefore];

    
    constructor(
        private adminService: AdminService
    ) {}

    ngOnInit() {
  }

 
  public updateSettings() {
      this.adminService.updateSettings(this.setList).subscribe(item => { console.log(item) },(error) => console.log(error));
  }

    
}
