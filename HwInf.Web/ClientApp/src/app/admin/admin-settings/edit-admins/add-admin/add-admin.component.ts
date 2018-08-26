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

    public users: User[];
    public userDic: { [search: string]: User; } = {};
    public stringForDic: string[] = [];
    public form: FormGroup;
    public selectedUser: User;
    public selectedString: string;

    @Output() adminListUpdated = new EventEmitter<User>();


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
                console.log(next);

                this.users.filter(user => user.Role !== "Admin").forEach((user, index) => {
                    this.userDic[user.LastName + " " + user.Name + " (" + user.Uid + ")"] = user;
                    this.stringForDic[index] = user.LastName + " " + user.Name + " (" + user.Uid + ")";
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

    addverwalter() {
        this.selectedUser = this.userDic[this.selectedString];
        this.adminService.removeAdmin(this.selectedUser, 'Verwalter').subscribe(
            (success) => this.adminListUpdated.emit(this.selectedUser)
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
