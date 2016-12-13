import {Component, OnInit, NgZone, ViewChild} from '@angular/core';
import {AuthService} from "../shared/auth.service";
import {Router} from "@angular/router";
import {NgForm} from "@angular/forms";
import {User} from "../shared/user.model";
import {ModalComponent} from "../shared/modal/modal.component";
import {AuthGuard} from "../shared/auth.guard";

@Component({
  selector: 'hw-inf-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  @ViewChild(ModalComponent) errorMessage: ModalComponent;

  constructor(private authService: AuthService, private router: Router, private authGuard: AuthGuard) { }

  ngOnInit() {
    if (this.authGuard.canActivate()) {
      this.router.navigate(['/dashboard']);
    }
  }

  private login(form: NgForm) {
    let user: User = form.form.value;
    this.authService.login(user)
      .subscribe(
        (result: boolean) => {
          if (result) {
            this.router.navigate(['/dashboard']);
          }
        },
        (error) => {
          this.errorMessage.show('UID oder Password falsch.');
        }

      );
  }

  private navigateToDashboard() {
    this.router.navigate(['/dashboard']);
  }

}
