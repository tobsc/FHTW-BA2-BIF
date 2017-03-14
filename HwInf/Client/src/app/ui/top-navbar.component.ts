import { Component, OnInit } from '@angular/core';
import {AppComponent} from "../app.component";

@Component({
  selector: 'hwinf-top-navbar',
  templateUrl: './top-navbar.component.html',
  styleUrls: ['./top-navbar.component.scss']
})
export class TopNavbarComponent implements OnInit {

  private isCollapsed: boolean = false;
  constructor(private rootComp: AppComponent) {  }

  setClass() {
    this.isCollapsed = !this.isCollapsed;
    if ( this.isCollapsed ){
      this.rootComp.cssClass = 'nav-sm';
    }
    else {
      this.rootComp.cssClass = 'nav-md';
    }
  }

  ngOnInit() {

  }


  public toggleNaviation(): void {
    let body = document.getElementsByTagName('body')[0];

    body.classList.add
  }
}
