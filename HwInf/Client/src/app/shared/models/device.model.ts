import {DeviceType} from "./device-type.model";
import {User} from "./user.model";
import {Status} from "./status.model";
import {DeviceMeta} from "./device-meta.model";
export class Device {
    constructor(
        public DeviceId: number,
        public Name: string,
        public InvNum: string,
        public Marke: string,
        public Room: string,
        public DeviceType: DeviceType,
        public Owner: User,
        public Status: Status,
        public DeviceMeta: DeviceMeta[],
        public IsActive: boolean
    ) {}
}