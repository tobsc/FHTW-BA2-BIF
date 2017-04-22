import {OrderStatus} from "./order-status.model";
import {Device} from "./device.model";
export class OrderItem {

    constructor(
        public ItemId: number,
        public OrderStatus: OrderStatus,
        public Device: Device,
        public From: string,
        public To: string,
        public ReturnDate: string,
    ) {}
}