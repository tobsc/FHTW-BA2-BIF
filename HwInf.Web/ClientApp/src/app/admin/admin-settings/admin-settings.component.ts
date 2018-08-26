import { Component, OnInit } from '@angular/core';
import { AdminService } from "../../shared/services/admin.service";
import { Setting } from "../../shared/models/setting.model";


@Component({
  selector: 'hwinf-admin-settings',
  templateUrl: './admin-settings.component.html',
  styleUrls: ['./admin-settings.component.scss']
})
export class AdminSettingsComponent implements OnInit {

    public ssStart: Setting = {
        Key: "ss_start",
        Value: sessionStorage.getItem("ss_start")
    };
    public ssEnd: Setting = {
        Key: "ss_end",
        Value: sessionStorage.getItem("ss_end")
    };
    public wsStart: Setting = {
        Key: "ws_start",
        Value: sessionStorage.getItem("ws_start")
    };
    public wsEnd: Setting = {
        Key: "ws_end",
        Value: sessionStorage.getItem("ws_end")
    };
    public newOrder: Setting = {
        Key: "new_order_mail",
        Value: sessionStorage.getItem("new_order_mail")
    };
    public mailAccept_above: Setting = {
        Key: "accept_mail_above",
        Value: sessionStorage.getItem("accept_mail_above")
    }

    public mailAccept_below: Setting = {
        Key: "accept_mail_below",
        Value: sessionStorage.getItem("accept_mail_below")
    }

    public mailDecline_above: Setting = {
        Key: "decline_mail_above",
        Value: sessionStorage.getItem("decline_mail_above")
    }

    public mailDecline_below: Setting = {
        Key: "decline_mail_below",
        Value: sessionStorage.getItem("decline_mail_below")
    }

    public mailReminder: Setting = {
        Key: "reminder_mail",
        Value: sessionStorage.getItem("reminder_mail")
    }

    public daysBefore: Setting = {
        Key: "days_before_reminder",
        Value: sessionStorage.getItem("days_before_reminder")
    }

    public setList: Setting[] = [this.ssStart, this.ssEnd, this.wsStart, this.wsEnd, this.newOrder, this.mailAccept_above, this.mailAccept_below, this.mailDecline_above, this.mailDecline_below, this.mailReminder, this.daysBefore];


    constructor(
        public adminService: AdminService
    ) {}

    ngOnInit() {
  }


  public updateSettings() {
      this.adminService.updateSettings(this.setList).subscribe(item => { console.log(item) },(error) => console.log(error));
  }


}
