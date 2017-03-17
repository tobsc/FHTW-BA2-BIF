import { Component, OnInit } from '@angular/core';
import { Subscription, Observable } from "rxjs";
import { Device } from "../../shared/models/device.model";
import { CartService } from "../../shared/services/cart.service";
import { User } from "../../shared/models/user.model";
import { UserService } from "../../shared/services/user.service";
import { NgForm } from "@angular/forms";
import { CanActivate, Router, RouterModule, RouterStateSnapshot, ActivatedRouteSnapshot } from "@angular/router";



@Component({
  selector: 'hwinf-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {

    private devices: any[];
    private user: User;

    constructor(
        private cartService: CartService,
        private userService: UserService,
        private router: Router
    ) { }


    ngOnInit() {
        this.devices = this.cartService.getItems();
        this.userService.getUser().subscribe((data) => this.user=data);
        
  }

    public removeItem(index : number):void {
        this.cartService.removeItem(index);
    }

    private update(form: NgForm): void {
        if (this.user.Tel !== form.form.value.Tel) {
            this.user.Tel = form.form.value.Tel;
            this.userService.updateUser(this.user);
        }
    }
}
