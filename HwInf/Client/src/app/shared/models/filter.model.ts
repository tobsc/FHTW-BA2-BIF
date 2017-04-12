import {DeviceMeta} from "./device-meta.model";
export class Filter {
    constructor(
        public DeviceType: string,
        public Order: string,
        public OrderBy: string,
        public Offset: number,
        public Limit: number,
        public MetaQuery: DeviceMeta[]
    ) {}
}