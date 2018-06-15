import {Component, OnInit, OnDestroy} from '@angular/core';
import {OrderFormDataService} from "./shared/order-form-data.service";
import {OrderProcessService} from "./shared/order-process.service";
import {Subscription} from "rxjs";
import {Router} from "@angular/router";

@Component({
  selector: 'hwinf-order-process',
  templateUrl: './order-process.component.html',
  styleUrls: ['./order-process.component.scss'],
  providers: [OrderFormDataService, OrderProcessService]
})
export class OrderProcessComponent implements OnInit, OnDestroy {

  private step1status;
  private step2status;
  private step3status;

  private subs: Subscription[] = [];
  private classname: string[]=[];

  constructor(
      private orderProcessService: OrderProcessService,
      private router: Router
  ) { }

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
      case "/anfrage/bestaetigung":
        this.classname[1] = "done";
        this.classname[2] = "done";
        this.classname[3] = "done";
        break;
      default:
        break;
    }
    return this.classname[id];
  }





  ngOnInit() {
    this.subs.push(this.orderProcessService.getStatus(0).subscribe((data) => this.step1status = data));
    this.subs.push(this.orderProcessService.getStatus(1).subscribe((data) => this.step2status = data));
    this.subs.push(this.orderProcessService.getStatus(2).subscribe((data) => this.step3status = data));
  }

  ngOnDestroy(): void {
    this.subs.forEach(i => i.unsubscribe());
  }
}
