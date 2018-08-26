import { Component, OnInit, ViewChild } from '@angular/core';
import { DamagesListComponent } from "./damages-list/damages-list.component";
import { Damage } from "../../../shared/models/damage.model";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: 'hwinf-damages',
  templateUrl: './damages.component.html',
  styleUrls: ['./damages.component.scss']
})
export class DamagesComponent implements OnInit {
    @ViewChild(DamagesListComponent) public list: DamagesListComponent;

    constructor() { }

    ngOnInit() {
    }

    pushData(item: Damage) {
        this.list.pushData(item);
    }

}
