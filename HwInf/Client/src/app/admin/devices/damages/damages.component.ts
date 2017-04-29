import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: 'hwinf-damages',
  templateUrl: './damages.component.html',
  styleUrls: ['./damages.component.scss']
})
export class DamagesComponent implements OnInit {

    private invNum: string;

    constructor(
        ) { }

  ngOnInit() {
      
  }

}
