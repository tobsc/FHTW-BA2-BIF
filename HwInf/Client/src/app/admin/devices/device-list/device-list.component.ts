import {Component, OnInit, OnDestroy} from "@angular/core";
import {DeviceService} from "../../../shared/services/device.service";
import {Subscription} from "rxjs";
import {Device} from "../../../shared/models/device.model";
import {ActivatedRoute, Router} from "@angular/router";
import {Filter} from "../../../shared/models/filter.model";
import {DeviceType} from "../../../shared/models/device-type.model";
import {UserService} from "../../../shared/services/user.service";
import {User} from "../../../shared/models/user.model";
@Component({
    selector: 'hwinf-device-list',
    templateUrl: './device-list.component.html',
    styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit, OnDestroy {

    private currentPage: number = 1;
    private maxPages: number = 0;
    private subscription: Subscription;
    private devices: Device[];
    private filter: Filter;
    private isAscending: boolean = true;
    private deviceTypes: DeviceType[];
    private owners: User[];
    private totalItems: number;
    private itemsPerPage: number = 6;

    constructor(
        private userService: UserService,
        private deviceService: DeviceService,
        private route: ActivatedRoute,
        private router: Router
    ) {}

    public pageChanged(event: any): void {
        console.log('Page changed to: ' + event.page);
        console.log('Number items per page: ' + event.itemsPerPage);
        this.currentPage = event.page;
        this.fetchDataPagination()
    }
    ngOnInit() {
        this.filter = new Filter();
        this.filter.DeviceType = '';
        this.filter.Order = 'ASC';
        this.filter.OrderBy = 'name';
        this.filter.Limit = this.itemsPerPage;
        this.filter.Offset = (this.currentPage-1) * this.filter.Limit;


        this.deviceService.getDeviceTypes().subscribe(
            (data) => {
                this.deviceTypes = data;
            }
        );


        this.fetchData();
    }

    fetchData() {

        if (typeof(this.subscription) !== 'undefined') this.subscription.unsubscribe(); // hack: prevent queryParams subscription to fire multiple times TODO: find a non hacky way

        this.subscription = this.route.queryParams
            .map((params) => ({ page: params['page'], orderby: params['orderby'] }))
            .flatMap((params) => {
                if (params.page) {
                    let pagenumber = +params.page;
                    if (pagenumber > 0) {
                        this.currentPage = pagenumber ;
                        this.filter.Offset = (this.currentPage-1) * this.filter.Limit;
                    }
                }
                if ( params.orderby ) {
                    this.filter.OrderBy = params.orderby;
                }
                return this.deviceService.getFilteredDevices(this.filter).publishReplay().refCount();
            })
            .subscribe((data) => {
                this.maxPages = data.MaxPages;
                this.devices = data.Devices;
                this.totalItems = data.TotalItems;
            });
    }

    fetchDataPagination() {

        this.filter.Offset = (this.currentPage-1) * this.filter.Limit;
        return this.deviceService.getFilteredDevices(this.filter)
            .subscribe((data) => {
                this.maxPages = data.MaxPages;
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

    public onChangeOrder(orderBy : string = 'name') {
        this.isAscending = !this.isAscending;
        this.filter.Order = (this.isAscending) ? 'ASC' : 'DESC';
        this.filter.OrderBy = orderBy;
        this.fetchDataPagination();
    }

    public onDeviceTypeChange(val: string) {
        this.router.navigate(['/admin/geraete/verwalten'], {skipLocationChange: true, queryParams: {'page' : 1, 'orderby' : this.filter.OrderBy}});
        this.filter.DeviceType = val;
        this.fetchData();
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
        console.log('IM AM DESTROYED');
    }

}
