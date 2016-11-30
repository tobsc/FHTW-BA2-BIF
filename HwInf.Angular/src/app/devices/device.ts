export class Device {
  constructor(
    private id: number,
    private name: string,
    private invNum: string,
    private status: number,
    private type: string,
    private attributes: string[]
  ) {}

  public getID(): number {
    return this.id;
  }

  public getName(): string {
      return this.name;
  }

  public getInvNum(): string {
    return this.invNum;
  }

  public getStatus(): number {
    return this.status;
  }

  public getType(): string {
    return this.type;
  }

  public getAttributes(): string[] {
    return this.attributes;
  }
}
