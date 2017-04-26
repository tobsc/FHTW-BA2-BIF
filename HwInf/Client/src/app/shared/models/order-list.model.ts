import {Order} from "./order.model";
export class OrderList {
    constructor(
        public MaxPages: number,
        public Results: number,
        public Orders: Order[],
    ) { }
}