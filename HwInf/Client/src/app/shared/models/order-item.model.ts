import {OrderStatus} from "./order-status.model";
import {Device} from "./device.model";
import {User} from "./user.model";
export class OrderItem {
    public ItemId: number;
    public OrderStatus: OrderStatus;
    public Device: Device;
    public From: string;
    public To: string;
    public ReturnDate: string;
    public CreateDate: string;
    public Entleiher: User;
    public IsDeclined: boolean;
    constructor() {}
}