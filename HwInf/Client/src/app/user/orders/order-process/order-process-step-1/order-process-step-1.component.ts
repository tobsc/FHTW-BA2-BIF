import {Component, OnInit, OnDestroy} from '@angular/core';
import {OrderFormDataService} from "../shared/order-form-data.service";
import {Order} from "../../../../shared/models/order.model";
import {UserService} from "../../../../shared/services/user.service";
import {User} from "../../../../shared/models/user.model";
import {Router, ActivatedRoute} from "@angular/router";
import {OrderProcessService} from "../shared/order-process.service";
var moment = require('moment');



@Component({
  selector: 'hwinf-order-process-step-1',
  templateUrl: './order-process-step-1.component.html',
  styleUrls: ['./order-process-step-1.component.scss'],
})
export class OrderProcessStep1Component implements OnInit, OnDestroy {
  private readonly DATE_FORMAT: string = 'DD. MMMM YYYY';
  private dateRangeString: string;

  /**
   * Daterangepicker options
   */
  private options: any = {
    autoUpdateInput: false,
    locale: { format: this.DATE_FORMAT },
    alwaysShowCalendars: false,
    minDate: moment().add(7, 'days').format(this.DATE_FORMAT),
    maxDate: moment('30. Juni', 'DD. MMMM').format(this.DATE_FORMAT),
    startDate: moment().add(7, 'days').format(this.DATE_FORMAT),
    endDate: moment().add(14, 'days').format(this.DATE_FORMAT),
    isInvalidDate: (date) =>  (date.format('dddd') === 'Sunday')
  };

  private order: Order;
  private user: User = new User();

  constructor(
      private orderProcessService: OrderProcessService,
      private formdataService: OrderFormDataService,
      private userService: UserService,
      private router: Router,
  ) { }

  ngOnInit() {

    this.order = this.formdataService.getData();
    this.userService.getUser()
        .subscribe((data) => {
          this.user = data;
        });

    if (!!this.order.From && !!this.order.To) {

      let startDate = moment(this.order.From).format(this.DATE_FORMAT);
      let endDate = moment(this.order.To).format(this.DATE_FORMAT);

      this.dateRangeString = `${startDate} - ${endDate}`;
      this.options.startDate = startDate;
      this.options.endDate = endDate;
    }
  }

  /**
   * update user data if number was changed or entered
   */
  private onNumberChange(): void {
    this.userService.updateUser(this.user).subscribe(
        (next) => console.log(next),
        (err) => console.log(err)
    );
  }

  /**
   * if a date was chosen update the input field with the date
   * @param ev
   */
  private onApplyDate(ev): void {
    let startDate = ev.picker.startDate;
    let endDate = ev.picker.endDate;
    this.dateRangeString = `${startDate.format(this.DATE_FORMAT)} - ${endDate.format(this.DATE_FORMAT)}`;
    this.order.From = startDate.format();
    this.order.To = endDate.format();
  }

  onSubmit() {
    this.orderProcessService.setStatus(0, 'done');
    this.router.navigate(['/anfrage/schritt-2']);
  }

  /**
   * store data in Service
   */
  ngOnDestroy() {
    this.formdataService.setData(this.order);
  }
}
