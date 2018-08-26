import { Component, OnInit } from '@angular/core';
import { UserService } from "../../shared/services/user.service";
import { User } from "../../shared/models/user.model";
import { AdminService } from "../../shared/services/admin.service";
import { Router } from "@angular/router";
import { FormBuilder, FormGroup, FormArray, Validators, NgForm } from "@angular/forms";

@Component({
  selector: 'hwinf-login-as',
  templateUrl: './login-as.component.html',
  styleUrls: ['./login-as.component.scss']
})
export class LoginAsComponent implements OnInit {

    public users: User[];
    public userDic: { [search: string]: User; } = {};
    public stringForDic: string[] = [];
    public form: FormGroup;
    public selectedUser: User;
    public selectedString: string;


    constructor(
        public userService: UserService,
        public fb: FormBuilder,
        public adminService: AdminService,
        public router: Router,
    ) { }

    ngOnInit() {
        this.init();
    }

    init() {

        
        this.userService.getUsers()
            .subscribe(
            (next) => {
                this.users = next; 

                this.users.forEach((user, index) => {
                    this.userDic["(" + user.Uid + ") " + user.LastName + " " + user.Name] = user;
                    this.stringForDic[index] = "(" + user.Uid + ") " + user.LastName + " " + user.Name;
                }
                );
                this.selectedUser = this.users[0];
            },
            (error) => console.log(error)
            );

        
    }
    
    
    impersonate() {
        this.selectedUser = this.userDic[this.selectedString];
        this.adminService.impersonate(this.selectedUser);
        this.router.navigate(["/dashboard"]);
        
        
    }

    isValid(): boolean {
        if (!!this.selectedString) {
            return false;
        }
        else {
            return true;
        }
    }


}
