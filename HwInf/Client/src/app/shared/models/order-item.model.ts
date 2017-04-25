import {OrderStatus} from "./order-status.model";
import {Device} from "./device.model";
import {User} from "./user.model";
export class OrderItem {

    constructor(
        public ItemId: number = -1,
        public OrderStatus: OrderStatus = null,
        public Device: Device = null,
        public From: string = null,
        public To: string = null,
        public ReturnDate: string = null,
        public CreateDate: string = null,
        public Entleiher: User = null,
        public IsDecline: boolean = false
    ) {}
}