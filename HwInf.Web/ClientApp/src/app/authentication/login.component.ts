import { Component, OnInit } from '@angular/core';
import {AuthService} from "./auth.service";
import {Router} from "@angular/router";
import {User} from "../shared/models/user.model";
import {NgForm} from "@angular/forms";
import { ErrorHandlerService } from "../shared/services/error-handler.service";
import { Error } from "../shared/models/error.model";

@Component({
  selector: 'hwinf-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {


  constructor(private authService: AuthService,
      private router: Router,
      private errorService: ErrorHandlerService,
  ) { }

  ngOnInit() {
      if ( this.authService.isLoggedIn() ) {
          this.goToDashboard()
      } else {
          this.authService.logout();
      }
  }

  private login(form: NgForm): void {
    let user: User = form.form.value;
    this.authService.login(user)
        .subscribe(
            (result: boolean) => {
              if (result) {
                this.goToDashboard();
              }
            },
            (error) => {
                this.errorService.showError(<Error>{ header: error.status + ' - ' + error.statusText, body:error['_body'], });
            }
        );
  }

  private goToDashboard(): void {
      this.router.navigate(['/dashboard']);
  }


}
