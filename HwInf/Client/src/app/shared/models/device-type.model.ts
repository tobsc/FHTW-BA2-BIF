import {DeviceComponent} from "./component.model";
export class DeviceType {
    constructor(
        public DeviceTypeId: number,
        public TypeName: string,
        public Components: DeviceComponent[]
    ) {}
}
