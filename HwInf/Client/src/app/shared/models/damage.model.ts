import { User } from "./user.model";
import { Device } from "./device.model";
import { DamageStatus } from "./damage-status.model";

export class Damage {
    public DamageId: number;
    public Date: Date;
    public Cause: User;
    public Reporter: User;
    public Description: string;
    public Device: Device;
    public DamageStatus: DamageStatus;
    public OrderId: number;
    constructor( ) { }
}