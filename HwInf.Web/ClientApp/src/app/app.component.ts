import {Component, HostBinding} from '@angular/core';

@Component({
  selector: 'body',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  @HostBinding('class') public cssClass = 'nav-md';
}
