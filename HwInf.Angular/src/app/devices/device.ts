export class Device {
  constructor(
    public DeviceId: number,
    public Name: string,
    public InvNum: string,
    public Status: number,
    public TypeId: string,
    public DeviceMetaData: string[]
  ) {}
}
