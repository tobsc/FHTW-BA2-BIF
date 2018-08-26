import {Component, OnInit, OnDestroy} from '@angular/core';
import {OrderFormDataService} from "../shared/order-form-data.service";
import { Order } from "../../../../shared/models/order.model";
import { Device } from "../../../../shared/models/device.model";
import { UserService } from "../../../../shared/services/user.service";
import { AdminService } from "../../../../shared/services/admin.service";
import { CartService } from "../../../../shared/services/cart.service";
import { JwtService } from "../../../../shared/services/jwt.service";
import { Setting } from "../../../../shared/models/setting.model";
import { User } from "../../../../shared/models/user.model";
import { VerwalterGuard } from "../../../../authentication/verwalter.guard";
import {Router, ActivatedRoute} from "@angular/router";
import {OrderProcessService} from "../shared/order-process.service";

@Component({
  selector: 'hwinf-order-process-step-1',
  templateUrl: './order-process-step-1.component.html',
  styleUrls: ['./order-process-step-1.component.scss'],
})
export class OrderProcessStep1Component implements OnInit, OnDestroy {
  public readonly DATE_FORMAT: string = 'DD.MM.YYYY';
  public now: Date = new Date();
  public dateRangeString: string;
  public dateSettings: Setting[] = this.adminService.getSettings();
  public semester: string = this.configureSemester();
  public semesterStart: string = this.configureSemesterStart();
  public semesterEnd: string = this.configureSemesterEnd();
  public semesterStartDate: Date = this.convertSemesterStartToDate();



  public order: Order;
  public user: User = new User();
  public users: User[];
  public items: Device[];
  public userDic: { [search: string]: User; } = {};
  public stringForDic: string[] = [];
  public selectedUser: User = new User();
  public selectedString: string;


  constructor(
      public orderProcessService: OrderProcessService,
      public formdataService: OrderFormDataService,
      public cartService: CartService,
      public userService: UserService,
      public adminService: AdminService,
      public router: Router,
      public jwtService: JwtService,
      public verwalterGuard: VerwalterGuard
  ) {
  }

    //ich glaub das wird vorm ngoninit erstellt, weil es ja bei der deklaration bereits bef�llt wird mit daten? aber theoretisch gehts so
    // also es erkennt richtig ob du oder ein nonadmin angemeldet is also scheint es so vorm ngoninit zu initialisieren
 /**
* Daterangepicker options
*/
  public options: any = {
      autoUpdateInput: true,
      locale: {
          format: this.DATE_FORMAT,
          applyLabel: "Bestätigen",
          cancelLabel: "Abbrechen"
      },
      alwaysShowCalendars: false,
      minDate: this.jwtService.isLoggedInAs() ? false : new Date() > this.semesterStartDate ? new Date() : this.semesterStart,
      maxDate: this.jwtService.isLoggedInAs() ? false : this.semesterEnd,
      startDate: new Date(),
      endDate: new Date().setDate(new Date().getDate() + 7),
  };

  ngOnInit() {
      this.order = this.formdataService.getData();
      console.log(this.order);
    this.userService.getUser()
        .subscribe((data) => {
            this.user = data;

            if (this.verwalterGuard.canActivate()) {
                this.userService.getUsers()
                    .subscribe(
                    (next) => {
                        this.users = next;

                        this.users.forEach((user, index) => {
                            this.userDic["(" + user.Uid + ") " + user.LastName + " " + user.Name] = user;
                            this.stringForDic[index] = "(" + user.Uid + ") " + user.LastName + " " + user.Name;
                        }
                        );
                        this.selectedString = "(" + this.user.Uid + ") " + this.user.LastName + " " + this.user.Name
                        this.selectedUser = this.user;
                    },
                    (error) => console.log(error)
                    );
            }
            else {
                this.selectedString = "(" + this.user.Uid + ") " + this.user.LastName + " " + this.user.Name
                this.stringForDic[0] = this.selectedString;
                this.userDic[this.selectedString] = this.user;
                this.selectedUser = this.user;
            }
        });



    if (!!this.order.From && !!this.order.To) {

      let startDate = this.order.From;
      let endDate = this.order.To;

      this.dateRangeString = `${startDate} - ${endDate}`;
      this.options.startDate = startDate;
      this.options.endDate = endDate;
    }
  }

  public convertSemesterStartToDate(): Date {
      var pieces = this.semesterStart.split('.');
      var datestring = pieces[2] + "-" + pieces[1] + "-" + pieces[0] + "T00:00:00";
      this.semesterStartDate = new Date(datestring);
      return this.semesterStartDate;
  }

  /**
   * update user data if number was changed or entered
   */
  public onNumberChange(): void {
    this.userService.updateUser(this.selectedUser).subscribe(
        (next) => console.log(next),
        (err) => console.log(err)
    );
  }

  public configureSemester(): string {
      var mm = this.now.getMonth() + 1;

      if (mm > 7) {
         return this.semester = "ws";
      }
      else {
          return this.semester = "ss";
      }
  }

  public configureSemesterStart(): string {
      var yyyy = this.now.getFullYear();
      return this.semesterStart = this.dateSettings.filter(item => item.Key == this.semester + "_start")[0].Value + "." + yyyy;
  }

  public configureSemesterEnd(): string {
      var yyyy = this.now.getFullYear();
      if (this.semester == "ss") {
          return this.semesterEnd = this.dateSettings.filter(item => item.Key == this.semester + "_end")[0].Value + "." + yyyy;
      }
      else {
          return this.semesterEnd = this.dateSettings.filter(item => item.Key == this.semester + "_end")[0].Value + "." + (this.now.getFullYear() + 1);
      }
  }


  /**
   * if a date was chosen update the input field with the date
   * @param ev
   */
  public onApplyDate(ev): void {
    let startDate = ev.picker.startDate;
    let endDate = ev.picker.endDate;
    this.dateRangeString = `${startDate.format(this.DATE_FORMAT)} - ${endDate.format(this.DATE_FORMAT)}`;
    this.order.From = startDate.format();
    this.order.To = endDate.format();
  }

  public isAdminOrVerwalter() {
      if (this.user.Role != "User") {
          return true;
      }

      return false;
  }
  public checkInvNum(): boolean {
      this.items = this.cartService.getItems();
      for (let item of this.items) {
          if (item.InvNum) {
              return true;
          }
      }
      return false;
  }

  onUserChange($event) {
      this.selectedUser = this.userDic[this.selectedString];
  }

  onSubmit() {
      //set user correctly
      if (this.verwalterGuard.canActivate()) {
          this.selectedUser = this.userDic[this.selectedString];
      }
      this.order.Entleiher = this.selectedUser;
      this.orderProcessService.setStatus(0, 'done');

      if (this.checkInvNum()) {
          this.router.navigate(['/anfrage/schritt-2']);
      }
      else
      {
          this.orderProcessService.setStatus(1, 'done');
          this.router.navigate(['/anfrage/schritt-3']);
      }


  }

  /**
   * store data in Service
   */
  ngOnDestroy() {
    this.formdataService.setData(this.order);
  }


}
