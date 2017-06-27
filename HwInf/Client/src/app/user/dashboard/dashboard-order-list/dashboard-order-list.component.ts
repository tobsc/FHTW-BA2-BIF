import { Component, OnInit } from '@angular/core';
import { OrderService } from "../../../shared/services/order.service";
import { Order } from "../../../shared/models/order.model";
import { OrderFilter } from "../../../shared/models/order-filter.model";
var moment = require('moment');
moment.locale('de');

@Component({
  selector: 'hwinf-dashboard-order-list',
  templateUrl: './dashboard-order-list.component.html',
  styleUrls: ['./dashboard-order-list.component.scss']
})
export class DashboardOrderListComponent implements OnInit {

    private orders: Order[] = [];
    private isAscending: boolean = true;
    private totalItems: number;
    private orderBy: string = 'date';
    private order: string = "DESC";
    private filter: OrderFilter;
    private hasMoreItems: boolean;

    constructor(private orderService: OrderService) { }

    ngOnInit(): void {

        this.filter = new OrderFilter();
        this.filter.StatusSlugs = ['offen', 'akzeptiert', 'ausgeliehen', 'abgelehnt'];
        this.filter.Order = this.order;
        this.filter.OrderBy = this.orderBy;
        this.filter.Limit = 20;
        this.filter.Offset = 0;

        this.fetchData();
    }

    fetchData() {

        this.orderService.getFilteredOrders(this.filter)
            .subscribe(
                data => {
                    this.orders = data.Orders.filter(i => {
                        var dateFrom = moment(i.ReturnDate)
                            .add(7, 'd').format('YYYY-MM-DD');
                        return (i.OrderStatus.Slug == 'abgelehnt' && moment().isBefore(dateFrom))
                            || i.OrderStatus.Slug != 'abgelehnt';
                    });
                    if (this.orders.length > 5)
                    {
                        this.hasMoreItems = true;
                        this.orders = this.orders.slice(0, 5);
                    }
                      
                }
            );
    }
}

