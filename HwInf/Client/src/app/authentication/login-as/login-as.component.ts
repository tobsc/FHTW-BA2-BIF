import { Component, OnInit } from '@angular/core';
import { UserService } from "../../shared/services/user.service";
import { User } from "../../shared/models/user.model";
import { ImpersonateService } from "../../shared/services/impersonate.service";
import { Router } from "@angular/router";
import { FormBuilder, FormGroup, FormArray, Validators, NgForm } from "@angular/forms";

@Component({
  selector: 'hwinf-login-as',
  templateUrl: './login-as.component.html',
  styleUrls: ['./login-as.component.scss']
})
export class LoginAsComponent implements OnInit {

    private users: User[];
    private form: FormGroup;
    private selectedUser: User;

    constructor(
        private userService: UserService,
        private fb: FormBuilder,
        private impersonateService: ImpersonateService,
        private router: Router,
    ) { }

    ngOnInit() {
        this.init();
    }

    init() {

        
        this.userService.getOwners()
            .subscribe(
            (next) => {
                this.users = next; 
                this.selectedUser = this.users[0];
            },
            (error) => console.log(error)
            );

        
    }

    
    impersonate() {
        this.impersonateService.impersonate(this.selectedUser)
        
    }

    onChange(value) {
        this.selectedUser = this.users.find(p => p.Uid == value);

    }


}
