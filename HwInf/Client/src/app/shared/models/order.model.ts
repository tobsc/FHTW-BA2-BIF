import {OrderItem} from "./order-item.model";
import {User} from "./user.model";
import {OrderStatus} from "./order-status.model";
export class Order {
    constructor(
        public OrderId: number = null,
        public Date: Date = null,
        public From: Date = null,
        public To: Date = null,
        public Entleiher: User = null,
        public Verwalter: User = null,
        public OrderItems: OrderItem[] = null,
        public OrderGuid: string = null,
        public OrderReason: string = null,
        public ReturnDate: Date = null,
        public OrderStatus: OrderStatus = null,
    ) {}
}