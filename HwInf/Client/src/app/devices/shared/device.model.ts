export class Device {
  constructor(
    public DeviceId: number,
    public Name: string,
    public Marke: string,
    public InvNum: string,
    public Status: string,
    public StatusId: string,
    public Type: string,
    public TypeId: string,
    public Room: string,
    public RoomId: number,
    public Owner: string,
    public OwnerUid: string,
    public DeviceMetaData: string[]
  ) {}
}
