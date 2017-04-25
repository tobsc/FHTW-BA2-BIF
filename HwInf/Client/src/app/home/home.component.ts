import {Component, OnInit, OnDestroy} from '@angular/core';

@Component({
  selector: 'hwinf-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy{
  ngOnDestroy(): void {

    console.log('I AM DESTROYED');
  }

  constructor() { }

  ngOnInit() {

    console.log("I AM CREATED");

  }

}
