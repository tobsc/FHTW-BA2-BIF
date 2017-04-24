import {DeviceMeta} from "./device-meta.model";
export class Filter {
    constructor(
        public DeviceType: string = '',
        public Order: string = '',
        public OrderBy: string = '',
        public Offset: number = -1,
        public Limit: number = -1,
        public MetaQuery: DeviceMeta[] = [],
        public IsVerwalterView = false,
        public OnlyActive = true
    ) {}
}