import { DeviceType } from "./device-type.model";
import { User } from "./user.model";
import { Status } from "./status.model";
import { DeviceMeta } from "./device-meta.model";
export class Field {
    constructor(
        public FieldId: number,
        public Name: string,
        public Label: string,
        public FieldType: string
    ) { }
}