import { Component, OnInit } from '@angular/core';
import {DeviceService} from "../../shared/services/device.service";
import {Observable} from "rxjs";
import {DeviceType} from "../../shared/models/device-type.model";
import { AdminGuard } from "../../authentication/admin.guard";
import { VerwalterGuard } from "../../authentication/verwalter.guard";
import { CanActivate, Router, RouterStateSnapshot, ActivatedRouteSnapshot } from "@angular/router";



@Component({
  selector: 'hwinf-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit, CanActivate {

  private deviceTypes: Observable<DeviceType[]>;

  constructor(
      private deviceService: DeviceService,
      private router: Router,
      private adminGuard: AdminGuard,
      private verwalterGuard: VerwalterGuard
  ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> |
        boolean {
        return this.adminGuard.canActivate() || this.verwalterGuard.canActivate();
    }

    ngOnInit() {
    this.deviceTypes = this.deviceService.getDeviceTypes();
  }

}
