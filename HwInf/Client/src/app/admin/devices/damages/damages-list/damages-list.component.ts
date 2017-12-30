import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { DeviceService } from "../../../../shared/services/device.service";
import { DamageService } from "../../../../shared/services/damage.service";
import { Damage } from "../../../../shared/models/damage.model";
import { ActivatedRoute, Router, Params } from "@angular/router";


@Component({
  selector: 'hwinf-damages-list',
  templateUrl: './damages-list.component.html',
  styleUrls: ['./damages-list.component.scss']
})
export class DamagesListComponent implements OnInit {
    
    private rows: any[];
    private damages: Damage[];
    private deviceId: number;
    private showDeviceInvNum: boolean=false;

  constructor(
      private deviceService: DeviceService,
      private damageService: DamageService,
      private route: ActivatedRoute,
      ) { }

  ngOnInit() {
      this.route.params.subscribe((params: Params) => {
          this.deviceId = params['id'];
          if (this.deviceId) {
              this.damageService.getDamagesByDeviceId(this.deviceId).subscribe((data) => {
                  this.damages = data;
                  this.rows = data.map(i => ({ isCollapsed: true, damage: i }));
              });
          }
          else {
              this.showDeviceInvNum = true;
              this.damageService.getDamages().subscribe((data) => {
                  this.damages = data;
                  this.rows = data.map(i => ({ isCollapsed: true, damage: i }));
              });
          }
        });
  }

  pushData(dmg: Damage) {
      this.damages.unshift(dmg);
      this.rows.unshift({ isCollapsed: true, damage: dmg });
  }

  onSave(i, damage) {
      this.damageService.updateDamage(damage)
          .subscribe(
          (success) => {
              //updates the values that were sent in response (Description, Cause and DamageStatus)
              this.rows[i].damage.Description = success.Description;
              this.rows[i].damage.Cause = success.Cause;
              this.rows[i].damage.DamageStatus = success.DamageStatus;
              this.rows[i].isCollapsed = true;
          },
          (error) => console.log(error)
          );
  }
}
