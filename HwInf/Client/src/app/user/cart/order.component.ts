import { Component, OnInit } from '@angular/core';
import { Subscription, Observable } from "rxjs";
import { Device } from "../../shared/models/device.model";
import { CartService } from "../../shared/services/cart.service";
import { User } from "../../shared/models/user.model";
import { UserService } from "../../shared/services/user.service";
import { NgForm } from "@angular/forms";
import { CanActivate, Router, RouterModule, RouterStateSnapshot, ActivatedRouteSnapshot } from "@angular/router";


@Component({
  selector: 'hwinf-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {

    private devices: Device[];
    private user: User;
    private classname: string[]=[];

    constructor(
        private cartService: CartService,
        private userService: UserService,
        private router: Router
    ) { }


    ngOnInit() {
        this.devices = this.cartService.getItems();
        this.userService.getUser().subscribe((data) => this.user = data);

    }

    getClass(id: number): string {
        switch (this.router.url) {
            case "/anfrage/schritt-1":
                this.classname[1] = "selected";
                this.classname[2] = "disabled";
                this.classname[3] = "disabled";
                break;
            case "/anfrage/schritt-2":
                this.classname[1] = "done";
                this.classname[2] = "selected";
                this.classname[3] = "disabled";
                break;
            case "/anfrage/schritt-3":
                this.classname[1] = "done";
                this.classname[2] = "done";
                this.classname[3] = "selected";
                break;
            default:

                break;

        }
        //console.log(this.router.url);
        //console.log(this.classname[id]);
        return this.classname[id];
    }
    
}
