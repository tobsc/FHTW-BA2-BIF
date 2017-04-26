import {Component, OnInit, Input} from "@angular/core";
import {OrderService} from "../../../shared/services/order.service";
import {Order} from "../../../shared/models/order.model";
import {OrderList} from "../../../shared/models/order-list.model";
import {OrderFilter} from "../../../shared/models/order-filter.model";
import {BehaviorSubject} from "rxjs";
var moment = require('moment');
moment.locale('de');


@Component({
    selector: 'hwinf-admin-order-list',
    templateUrl: './admin-order-list.component.html',
    styleUrls: ['./admin-order-list.component.scss']
})
export class AdminOrderListComponent implements OnInit {

    private _filter = new BehaviorSubject<OrderFilter>(new OrderFilter());

    @Input()
    private set filter(value) {
        this._filter.next(value);
    };

    private orders: Order[] = [];

    constructor(private orderService: OrderService) { }

    ngOnInit() {
        this._filter
            .flatMap((filter) => this.orderService.getFilteredOrders(filter))
            .subscribe((data: OrderList) => {
                this.orders = data.Orders;
            });
    }

    updateOrder(index: number, order: Order) {
        this.orders[index] = order;
    }

}
