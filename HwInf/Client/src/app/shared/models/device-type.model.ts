import {DeviceComponent} from "./component.model";
export class DeviceType {
    constructor(
        public DeviceTypeId: number,
        public Name: string,
        public Slug: string,
        public PermaLink
    ) {}
}
