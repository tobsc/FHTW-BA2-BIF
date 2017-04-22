import {OrderItem} from "./order-item.model";
export class Order {
    constructor(
        public OrderId: number,
        public Date: Date,
        public From: Date,
        public To: Date,
        public EntleiherUid: string,
        public VerwalterUid: string,
        public OrderItems: OrderItem[],
        public OrderGuid: string,
        public OrderReason: string
    ) {}
}