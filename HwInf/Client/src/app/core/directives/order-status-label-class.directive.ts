import {Directive, HostBinding, Input, OnInit} from '@angular/core';
import {OrderStatus} from "../../shared/models/order-status.model";

@Directive({
  selector: '[hwinfOrderStatusLabelClass]'
})
export class OrderStatusLabelClassDirective implements OnInit{

  @HostBinding('class') private currentClass : string;
  @HostBinding('style.color') private color : string = '#fff';
  @Input() orderStatus: OrderStatus;

  constructor() {
  }
  ngOnInit(): void {

    switch (this.orderStatus.Slug) {
      case 'offen':
        this.currentClass = 'label label-warning';
      break;

      case 'akzeptiert':
        this.currentClass = 'label label-success';
      break;

      case 'ausgeliehen':
        this.currentClass = 'label label-info';
      break;

      case 'abgelehnt':
        this.currentClass = 'label label-danger';
      break;

      case 'abgeschlossen':
      default:
        this.currentClass = 'label label-default';
      break;
    }
  }


}
