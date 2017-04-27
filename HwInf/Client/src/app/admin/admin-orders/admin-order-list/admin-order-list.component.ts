import {Component, OnInit, Input} from "@angular/core";
import {OrderService} from "../../../shared/services/order.service";
import {Order} from "../../../shared/models/order.model";
import {OrderList} from "../../../shared/models/order-list.model";
import {OrderFilter} from "../../../shared/models/order-filter.model";
import {BehaviorSubject} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
var moment = require('moment');
moment.locale('de');


@Component({
    selector: 'hwinf-admin-order-list',
    templateUrl: './admin-order-list.component.html',
    styleUrls: ['./admin-order-list.component.scss']
})
export class AdminOrderListComponent implements OnInit {

    private _filter = new BehaviorSubject<OrderFilter>(new OrderFilter());
    private maxPages: number = -1;
    private currentPage: number = 1;

    @Input()
    private set filter(value) {
        this._filter.next(value);
    };

    private orders: Order[] = [];

    constructor(
        private orderService: OrderService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit() {
        this.route.queryParams
            .do(params => {
                if(!!params['page'])
                    this.currentPage = +params['page']
            })
            .flatMap(i => this._filter)
            .flatMap(filter => {
                let tmpFilter = filter;
                tmpFilter.Limit = 10;
                tmpFilter.Offset = (this.currentPage-1) * tmpFilter.Limit;
                return this.orderService.getFilteredOrders(tmpFilter);
            })
            .subscribe((data: OrderList) => {
                this.orders = data.Orders;
                this.maxPages = data.MaxPages;
            });
    }



    updateOrder(index: number, order: Order) {
        //this.orders.splice(index,1);
        this.orders[index] = order;
    }

}
