import {Component, HostBinding } from '@angular/core';

@Component({
    selector: 'body',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
})
export class AppComponent  {
    constructor() {}
    @HostBinding('class') public cssClass = 'nav-md';
}
