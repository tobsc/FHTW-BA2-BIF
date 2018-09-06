import {Order} from "./order.model";
export class OrderList {
    constructor(
        public MaxPages: number,
        public TotalItems: number,
        public Orders: Order[],
    ) { }
}