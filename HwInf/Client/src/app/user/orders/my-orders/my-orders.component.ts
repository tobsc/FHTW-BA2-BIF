import { Component, OnInit } from '@angular/core';
import {OrderService} from "../../../shared/services/order.service";
import {Order} from "../../../shared/models/order.model";
import {OrderFilter} from "../../../shared/models/order-filter.model";

@Component({
  selector: 'hwinf-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.scss']
})
export class MyOrdersComponent implements OnInit {

  private orders: Order[] = [];
  private currentPage: number = 1;
  private isAscending: boolean = true;
  private totalItems: number;
  private itemsPerPage: number = 2;
  private orderBy: string = 'date';
  private order: string = "DESC";
  private maxSize: number = 8;
  private filter: OrderFilter;

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {

    this.filter = new OrderFilter();
    this.filter.StatusSlugs = ['offen', 'akzeptiert', 'ausgeliehen'];
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
              this.orders = data.Orders;
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

}
