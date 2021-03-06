import {Component, OnInit} from "@angular/core";
import {DeviceService} from "../../../shared/services/device.service";
import {Device} from "../../../shared/models/device.model";
import {ActivatedRoute, Router} from "@angular/router";
import {Filter} from "../../../shared/models/filter.model";
import {DeviceType} from "../../../shared/models/device-type.model";
import { UserService } from "../../../shared/services/user.service";
import { JwtService } from "../../../shared/services/jwt.service";
import {User} from "../../../shared/models/user.model";
import {SessionStorageService} from "../../../shared/services/session-storage.service";
@Component({
    selector: 'hwinf-device-list',
    templateUrl: './device-list.component.html',
    styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit {

    private currentPage: number = 1;
    private devices: Device[];
    private filter: Filter;
    private isAscending: boolean;
    private deviceTypes: DeviceType[];
    private owners: User[];
    private totalItems: number;
    private itemsPerPage: number = 25;
    private orderBy: string;
    private maxSize: number = 8;

    constructor(
        private userService: UserService,
        private deviceService: DeviceService,
        private jwtService: JwtService,
        private route: ActivatedRoute,
        private router: Router,
        private sessionStorageService: SessionStorageService
    ) {}

    public pageChanged(event: any): void {
        this.currentPage = event.page;
        this.fetchData()
    }

    ngOnInit() {
        this.orderBy = this.sessionStorageService.getSortingSetting();
        this.isAscending = this.sessionStorageService.getSortingIsAscending();
        this.filter = new Filter();
        this.filter.DeviceType = this.sessionStorageService.getSortingDeviceType();;
        this.filter.Order = this.isAscending ? 'ASC': 'DESC';
        this.filter.OrderBy = this.orderBy;
        this.filter.Limit = this.itemsPerPage;
        this.filter.Offset = (this.currentPage - 1) * this.filter.Limit;
        this.filter.IsVerwalterView = this.jwtService.isVerwalter();

        console.log(this.filter.DeviceType);
        this.deviceService.getDeviceTypes().subscribe(
            (data) => {
                this.deviceTypes = data;
            }
        );


        this.fetchData();
    }


    fetchData() {

        this.filter.Offset = (this.currentPage - 1) * this.filter.Limit;
        return this.deviceService.getFilteredDevices(this.filter)
            .subscribe((data) => {
                this.devices = data.Devices;
                this.totalItems = data.TotalItems;
            });
    }

    removeDevice(index: number) {
        this.devices.splice(index, 1);
    }

    onDelete( deviceId: number, index: number) {

        this.deviceService.deleteDevice(deviceId)
            .subscribe(
                () => { this.removeDevice(index) },
                (err) => console.log(err)
            );
    }

    public onChangeOrder(orderBy : string) {
        this.sessionStorageService.setSortingSetting(orderBy, !this.isAscending);
        this.isAscending = !this.isAscending;
        this.filter.Order = (this.isAscending) ? 'ASC' : 'DESC';
        this.filter.OrderBy = orderBy;
        this.orderBy = orderBy;
        this.fetchData();
    }

    public onDeviceTypeChange(val: string) {
        this.router.navigate(['/admin/geraete/verwalten'], {skipLocationChange: true, queryParams: {'page' : 1, 'orderby' : this.filter.OrderBy}});
        this.filter.DeviceType = val;
        this.sessionStorageService.setSortingDeviceType(val);
        this.fetchData();
    }

}
