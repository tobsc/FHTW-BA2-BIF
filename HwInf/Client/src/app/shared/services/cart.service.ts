import { Injectable } from '@angular/core';
import { Device } from "../models/device.model";
import { Subject, Observable } from "rxjs";
import { Router } from "@angular/router";
import { JwtService } from "./jwt.service";

@Injectable()
export class CartService {
    private amount: Subject<number> = new Subject<number>();
    private items: Device[] = [];
    private count: number = 0;
    

    constructor(private router: Router, private jwtService:JwtService) {
        if (!!localStorage.getItem('cart_list')) {
            this.items = JSON.parse(localStorage.getItem('cart_list' + this.getHash(jwtService.getUid())));
        }


    }

    public getItems() {
        return this.items;
    }

    public getHash(uid: string) {
        
            var hash = 0;
            if (uid.length == 0) return hash;
            for (let i = 0; i < uid.length; i++) {
                var char = uid.charCodeAt(i);
                hash = ((hash << 5) - hash) + char;
                hash = hash & hash; // Convert to 32bit integer
            }

            console.log('cart_list' + hash);
            return hash;
        

    }

    public getAmount(): Observable<number> {
        return this.amount.asObservable();
    }

    public addItem(item: Device) {
        if (!this.contains(item)) {
            this.items.push(item);
            this.updateData()
            this.count++;
        }
    }

    public removeItem(index: number) {
        this.items.splice(index, 1);
        this.updateData();
        this.count--;
    }

    private contains(item: Device): boolean {
        for (let device of this.items) {
            if (device.DeviceId == item.DeviceId) {
                return true;
            }
        }
        return false;
    }

    public clear() {
        this.items = [];
        this.updateData();
    }

    private updateData(): void {
        this.updateLocalStorage();
        this.updateAmount();
    }

    private updateLocalStorage(): void {
        localStorage.setItem('cart_list', JSON.stringify(this.items));
    }

    public updateAmount(): void {
        this.amount.next(this.items.length);
    }

   
}
