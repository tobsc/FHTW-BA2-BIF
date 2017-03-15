import { Component, OnInit } from '@angular/core';
import {AuthService} from "./auth.service";
import {Router} from "@angular/router";
import {User} from "../shared/models/user.model";
import {NgForm} from "@angular/forms";

@Component({
  selector: 'hwinf-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {


  constructor(private authService: AuthService,
              private router: Router
  ) { }

  ngOnInit() {
      if ( this.authService.isLoggedIn() ) {
          this.goToDashboard()
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
              console.log(error);
            }
        );
  }

  private goToDashboard(): void {
      this.router.navigate(['/dashboard']);
  }


}
