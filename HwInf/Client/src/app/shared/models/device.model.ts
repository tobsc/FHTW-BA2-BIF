import {DeviceType} from "./device-type.model";
import {User} from "./user.model";
import {Status} from "./status.model";
import {DeviceMeta} from "./device-meta.model";
import {FieldGroup} from "./fieldgroup.model";
export class Device {
    constructor(
        public DeviceId: number,
        public Name: string,
        public Notiz: string,
        public InvNum: string,
        public Marke: string,
        public Raum: string,
        public DeviceType: DeviceType,
        public Verwalter: User,
        public Status: Status,
        public DeviceMeta: DeviceMeta[],
        public IsActive: boolean,
        public FieldGroups: FieldGroup[],
        public CreateDate: Date
    ) {}
}