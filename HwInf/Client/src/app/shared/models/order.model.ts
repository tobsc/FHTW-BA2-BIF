import {OrderItem} from "./order-item.model";
export class Order {
    constructor(
        public OrderId: number = null,
        public Date: Date = null,
        public From: Date = null,
        public To: Date = null,
        public EntleiherUid: string = null,
        public VerwalterUid: string = null,
        public OrderItems: OrderItem[] = null,
        public OrderGuid: string = null,
        public OrderReason: string = null
    ) {}
}