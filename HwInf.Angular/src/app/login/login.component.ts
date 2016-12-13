import {Component, OnInit, NgZone} from '@angular/core';
import {AuthService} from "../shared/auth.service";
import {Router} from "@angular/router";
import {NgForm} from "@angular/forms";
import {User} from "../shared/user.model";

@Component({
  selector: 'hw-inf-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router, private zone: NgZone) { }

  ngOnInit() {
    this.authService.logout();
  }

  private login(form: NgForm) {
    let user: User = form.form.value;
    this.authService.login(user)
      .subscribe((result: boolean) => {

          if (result) {
            this.router.navigate(['/dashboard']);
            console.log('yes');
          }

      });
  }

  private navigateToDashboard() {
    this.router.navigate(['/dashboard']);
  }

}
