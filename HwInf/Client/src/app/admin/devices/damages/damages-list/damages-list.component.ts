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
    private invNum: string;

  constructor(
      private deviceService: DeviceService,
      private damageService: DamageService,
      private route: ActivatedRoute,
      ) { }

  ngOnInit() {
      this.route.params.subscribe((params: Params) => {
          this.invNum = params['invnum'];
          this.damageService.getDamagesByInvNum(this.invNum).subscribe((data) => {
              this.damages = data;
              this.rows = data.map(i => ({ isCollapsed: true, damage: i }));
              this.fetchData();
            });
        });
  }

  fetchData() {
      console.log(this.damages);
  }
}
