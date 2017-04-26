import { Component, OnInit } from '@angular/core';
import { AdminService } from '../shared/services/admin.service';

@Component({
  selector: 'hwinf-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

    constructor(
        private adminService: AdminService
    ) { 
        this.adminService.loadSemestreData();
    }

  ngOnInit() {
  }

}
