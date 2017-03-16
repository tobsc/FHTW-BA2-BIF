import { DeviceType } from "./device-type.model";
import { User } from "./user.model";
import { Status } from "./status.model";
import { DeviceMeta } from "./device-meta.model";
import { Field } from "./field.model";

export class Group {
    constructor(
        public GroupId: number,
        public Name: string,
        public Label: string,
        public Field: Field[]
    ) { }
}