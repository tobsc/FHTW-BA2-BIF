import {Component, OnInit, NgZone, ViewChild} from '@angular/core';
import {AuthService} from "./auth.service";
import {Router} from "@angular/router";
import {NgForm} from "@angular/forms";
import {UserCredentials} from "../shared/user-credentials.model";
import {ModalComponent} from "../shared/modal/modal.component";
import {AuthGuard} from "./auth.guard";
import {ErrorMessageService} from "../shared/error-message/error-message.service";
import {Modal} from "../shared/modal/modal.model";

@Component({
  selector: 'hw-inf-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private router: Router,
    private authGuard: AuthGuard,
    private errorMessageService: ErrorMessageService
  ) { }

  ngOnInit() {
    if (this.authGuard.canActivate()) {
      this.router.navigate(['/dashboard']);
    }
    else {
      this.authService.logout();
    }
  }

  private login(form: NgForm) {
    let user: UserCredentials = form.form.value;
    this.authService.login(user)
      .subscribe(
        (result: boolean) => {
          if (result) {
            this.router.navigate(['/dashboard']);
          }
        },
        (error) => {
          console.log(error);
          this.errorMessageService.showErrorMessage(<Modal>{
            header: error.status + ' - ' + error.statusText,
            body: error['_body'],
          });
        }

      );
  }

  private navigateToDashboard() {
    this.router.navigate(['/dashboard']);
  }

}
