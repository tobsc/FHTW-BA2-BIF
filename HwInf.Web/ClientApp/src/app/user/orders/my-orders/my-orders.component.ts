import { Component, OnInit } from '@angular/core';
import {OrderService} from "../../../shared/services/order.service";
import {Order} from "../../../shared/models/order.model";
import { OrderFilter } from "../../../shared/models/order-filter.model";
import {isBefore} from "ngx-bootstrap/chronos/utils/date-compare";

@Component({
  selector: 'hwinf-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.scss']
})
export class MyOrdersComponent implements OnInit {

  public orders: Order[] = [];
  public currentPage: number = 1;
  public isAscending: boolean = true;
  public totalItems: number;
  public itemsPerPage: number = 25;
  public orderBy: string = 'date';
  public order: string = "DESC";
  public maxSize: number = 8;
  public filter: OrderFilter;

  constructor(public orderService: OrderService) {}

  ngOnInit(): void {

    this.filter = new OrderFilter();
    this.filter.StatusSlugs = ['offen', 'akzeptiert', 'ausgeliehen', 'abgelehnt'];
    this.filter.Order = this.order;
    this.filter.OrderBy = this.orderBy;
    this.filter.Limit = this.itemsPerPage;
    this.filter.Offset = (this.currentPage-1) * this.filter.Limit;

    this.fetchData();
  }

  fetchData() {

    this.filter.Offset = (this.currentPage-1) * this.filter.Limit;
    this.orderService.getFilteredOrders(this.filter)
        .subscribe(
        data => {
          this.orders = data.Orders.filter(i => {
            var returnDate = new Date(i.ReturnDate);
                  var dateFrom = returnDate;
                  dateFrom.setDate(returnDate.getDate() + 7);
                  var dateNow = new Date();
                  return (i.OrderStatus.Slug == 'abgelehnt' && isBefore(dateNow, dateFrom))
                    || i.OrderStatus.Slug != 'abgelehnt';
                });
              this.totalItems = data.TotalItems;
            }
        )
  }

  public pageChanged(event: any): void {
    this.currentPage = event.page;
    this.fetchData()
  }

  public onChangeOrder(orderBy : string) {
    orderBy = JSON.parse(orderBy);
    this.isAscending = !this.isAscending;
    this.filter.Order = orderBy['o'];
    this.order = orderBy['o'];
    this.orderBy = orderBy['by'];
    this.filter.OrderBy = orderBy['by'];
    this.fetchData();
  }

    public onDeleteOrder(index: number) {
        this.orders.splice(index, 1);
    }

}
