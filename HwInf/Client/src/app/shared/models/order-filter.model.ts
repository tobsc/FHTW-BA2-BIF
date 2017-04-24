export class OrderFilter {
    public Order: string = "DESC";
    public OrderBy: string = "OrderStatus";
    public OrderByFallback: "CreateDate";
    public StatusQuery: string[] = [];
    public UidQuery: string[] = [];
    public IsIncoming: boolean = false;
    constructor() {}
}