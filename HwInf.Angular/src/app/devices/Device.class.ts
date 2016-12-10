export class Device {
    constructor(
        public DeviceId: number,
        public Name: string,
        public InvNum: string,
        public StatusId: number,
        public Status: string,
        public TypeId: string,
        public DeviceMetaData: string[]
    ) {}
}
