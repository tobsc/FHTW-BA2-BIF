export class Order {
  constructor(
    public OrderId: number,
    public Date: string,
    public From: string,
    public To: string,
    public Person: string,
    public PersonUid: string,
    public Owner: string,
    public OwnerUid: string,
    public OrderItems: number[],
    public Status: string,
    public StatusId: number
  ) {}
}
