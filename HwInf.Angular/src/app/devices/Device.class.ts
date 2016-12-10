export class Device {
  constructor(
    public DeviceId: number,
    public Description: string,
    public InvNum: string,
    public StatusId: string,
    public Status: string,
    public TypeId: string,
    public DeviceMetaData: string[]
  ) {}
}
