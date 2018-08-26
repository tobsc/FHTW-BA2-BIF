import { Component, OnInit } from '@angular/core';
import { OrderService } from "../../../shared/services/order.service";
import { Order } from "../../../shared/models/order.model";
import { OrderFilter } from "../../../shared/models/order-filter.model";
import {isBefore} from "ngx-bootstrap/chronos/utils/date-compare";

@Component({
  selector: 'hwinf-dashboard-order-list',
  templateUrl: './dashboard-order-list.component.html',
  styleUrls: ['./dashboard-order-list.component.scss']
})
export class DashboardOrderListComponent implements OnInit {

    public orders: Order[] = [];
    public isAscending: boolean = true;
    public totalItems: number;
    public orderBy: string = 'date';
    public order: string = "DESC";
    public filter: OrderFilter;
    public hasMoreItems: boolean;

    constructor(public orderService: OrderService) { }

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
            console.log(data);
            this.orders = data.Orders.filter(i => {
              var returnDate = new Date(i.ReturnDate);
                        var dateFrom = returnDate;
              dateFrom.setDate(returnDate.getDate() + 7);
                        var dateNow = new Date();
                        return (i.OrderStatus.Slug == 'abgelehnt' && isBefore(dateNow, dateFrom))
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

