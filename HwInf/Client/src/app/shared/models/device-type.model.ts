import {DeviceComponent} from "./component.model";
import {FieldGroup} from "./fieldgroup.model";
export class DeviceType {
    constructor(
        public DeviceTypeId: number,
        public Name: string,
        public Slug: string,
        public FieldGroups: FieldGroup,
        public PermaLink
    ) {}
}
