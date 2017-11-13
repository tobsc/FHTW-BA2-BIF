import {Component, OnInit, EventEmitter, Output} from '@angular/core';
import { UserService } from "../../../../shared/services/user.service";
import { User } from "../../../../shared/models/user.model";
import { AdminService } from "../../../../shared/services/admin.service";
import { Router } from "@angular/router";
import { FormBuilder, FormGroup, FormArray, Validators, NgForm } from "@angular/forms";

@Component({
  selector: 'hwinf-add-admin',
  templateUrl: './add-admin.component.html',
  styleUrls: ['./add-admin.component.scss']
})
export class AddAdminComponent implements OnInit {

    private users: User[];
    private userDic: { [search: string]: User; } = {};
    private stringForDic: string[] = [];
    private form: FormGroup;
    private selectedUser: User;
    private selectedString: string;

    @Output() adminListUpdated = new EventEmitter<User>();


    constructor(
        private userService: UserService,
        private fb: FormBuilder,
        private adminService: AdminService,
        private router: Router,
    ) { }

    ngOnInit() {
        this.init();
    }

    init() {


        this.userService.getUsers()
            .subscribe(
            (next) => {
                this.users = next;
                console.log(next);

                this.users.filter(user => user.Role !== "Admin").forEach((user, index) => {
                    this.userDic["(" + user.Uid + ") " + user.LastName + " " + user.Name] = user;
                    this.stringForDic[index] = "(" + user.Uid + ") " + user.LastName + " " + user.Name;
                }
                );
                this.selectedUser = this.users[0];
            },
            (error) => console.log(error)
            );

    }

    addadmin() {
        this.selectedUser = this.userDic[this.selectedString];
        console.log(this.selectedUser);
        this.adminService.addAdmin(this.selectedUser).subscribe(
            (success) => {
                this.adminListUpdated.emit(this.selectedUser);
                console.log("add" + success);
            }
        );
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
