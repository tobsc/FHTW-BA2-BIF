import {Component, OnInit, Input} from "@angular/core";
import {OrderService} from "../../../shared/services/order.service";
import {Order} from "../../../shared/models/order.model";
import {OrderList} from "../../../shared/models/order-list.model";
import {OrderFilter} from "../../../shared/models/order-filter.model";
var moment = require('moment');
moment.locale('de');


@Component({
    selector: 'hwinf-admin-order-list',
    templateUrl: './admin-order-list.component.html',
    styleUrls: ['./admin-order-list.component.scss']
})
export class AdminOrderListComponent implements OnInit {
    @Input() private filter: OrderFilter;
    private orders: Order[] = [];

    constructor(private orderService: OrderService) { }

    ngOnInit() {
        this.fetchData();
    }

    fetchData(): void {
        this.orderService.getFilteredOrders(this.filter)
            .subscribe((data: OrderList) => {
                console.log(data);
                this.orders = data.Orders;
            });
    }

    updateOrder(index: number, order: Order) {
        this.orders[index] = order;
    }

}
