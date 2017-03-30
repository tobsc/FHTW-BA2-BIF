import {Device} from "./device.model";
export class DeviceList {
    constructor(
        public CurrentPage: number,
        public MaxPages: number,
        public Devices: Device[],
    ) {}
}