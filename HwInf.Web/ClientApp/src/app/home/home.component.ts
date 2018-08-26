import {Component, OnInit, OnDestroy} from '@angular/core';
import { AdminService } from '../shared/services/admin.service';


@Component({
  selector: 'hwinf-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy{
  ngOnDestroy(): void {

  }

    constructor(
        public adminService: AdminService
    ) { 
        this.adminService.loadSemestreData();
    }

  ngOnInit() {


  }

}
