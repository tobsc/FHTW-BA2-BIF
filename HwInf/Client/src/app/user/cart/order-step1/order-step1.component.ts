import { Component, OnInit } from '@angular/core';
import { Subscription, Observable } from "rxjs";
import { Device } from "../../../shared/models/device.model";
import { CartService } from "../../../shared/services/cart.service";
import { User } from "../../../shared/models/user.model";
import { UserService } from "../../../shared/services/user.service";
import { NgForm } from "@angular/forms";
import { CanActivate, Router, RouterModule, RouterStateSnapshot, ActivatedRouteSnapshot } from "@angular/router";




@Component({
    selector: 'hwinf-order-step1',
    templateUrl: './order-step1.component.html',
    styleUrls: ['./order-step1.component.scss']
})
export class OrderStep1Component implements OnInit {

    private devices: Device[];
    private user: User;
    private date: string;

    constructor(
        private cartService: CartService,
        private userService: UserService,
        private router: Router
    ) { }


    ngOnInit() {
        this.devices = this.cartService.getItems();
        this.userService.getUser().subscribe((data) => this.user = data);

    }

    public removeItem(index: number): void {
        this.cartService.removeItem(index);
    }

    private update(form: NgForm): void {
        if (this.user.Tel !== form.form.value.Tel) {
            this.user.Tel = form.form.value.Tel;
            this.userService.updateUser(this.user);
        }
    }

    
}
