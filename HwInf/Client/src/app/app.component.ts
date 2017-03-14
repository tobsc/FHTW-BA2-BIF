import {Component, HostBinding, OnInit} from '@angular/core';
import {AuthService} from "./authentication/auth.service";

@Component({
    selector: 'body',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
})
export class AppComponent  {
    constructor(private authService: AuthService) {}
    @HostBinding('class') public cssClass = 'nav-md';
}
