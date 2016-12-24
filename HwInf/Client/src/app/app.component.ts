import { Component } from '@angular/core';
import {AuthService} from "./authentication/auth.service";

@Component({
    selector: 'hw-inf-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
})
export class AppComponent {
    constructor(private authService: AuthService) {}
}
