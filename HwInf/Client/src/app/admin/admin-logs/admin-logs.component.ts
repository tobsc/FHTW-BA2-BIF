import { Component, OnInit } from '@angular/core';
import {AdminService} from "../../shared/services/admin.service";

@Component({
  selector: 'hwinf-admin-logs',
  templateUrl: './admin-logs.component.html',
  styleUrls: ['./admin-logs.component.scss']
})
export class AdminLogsComponent implements OnInit {

  private logs: string[];

  constructor(
      private adminService: AdminService
  ) {}

  ngOnInit() {
    this.adminService.getLog().subscribe(
        (data) => {
          this.logs = data;
          console.log(this.logs);
    });
  }

}
