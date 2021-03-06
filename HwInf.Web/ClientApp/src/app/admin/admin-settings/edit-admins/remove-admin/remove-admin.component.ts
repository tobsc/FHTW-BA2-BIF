import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import { UserService } from "../../../../shared/services/user.service";
import { User } from "../../../../shared/models/user.model";
import { AdminService } from "../../../../shared/services/admin.service";
import { Router } from "@angular/router";
import { FormBuilder, FormGroup, FormArray, Validators, NgForm } from "@angular/forms";

@Component({
  selector: 'hwinf-remove-admin',
  templateUrl: './remove-admin.component.html',
  styleUrls: ['./remove-admin.component.scss']
})
export class RemoveAdminComponent implements OnInit {

    private users: User[];
    private userDic: { [search: string]: User; } = {};
    private stringForDic: string[] = [];
    private form: FormGroup;
    private selectedUser: User;
    private selectedString: string;
    private roleType;
    private role: string = null;


    constructor(
        private userService: UserService,
        private fb: FormBuilder,
        private adminService: AdminService,
        private router: Router,
    ) {
        this.roleType = {
            role: "User",
        }
    }

    ngOnInit() {
        this.init();
    }

    init() {


        this.userService.getOwners()
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

    addadmin(index) {
        this.selectedUser = this.users[index];
        console.log(this.selectedUser);
        this.adminService.addAdmin(this.selectedUser).subscribe(
            (success) => {
                console.log("add" + success);
                this.init();
            }
        );
    }

    removeadmin(index, role) {
        this.selectedUser = this.users[index];
        this.adminService.removeAdmin(this.selectedUser, role).subscribe((success) => this.init());
        this.users.splice(index, 1);
    }

    pushData(user: User) {
        this.users.push(user);
        console.log("pushed");
    }

    isValid(): boolean {
        if (!!this.selectedString && !!this.roleType.role) {
            return false;
        }
        else {
            return true;
        }
    }

}
