export class OrderFilter {
    public Order: string = "DESC";
    public OrderBy: string = "OrderStatus";
    public OrderByFallback: "Date";
    public IsAdminView: boolean = false;
    public StatusSlugs: string[] = [];
    public Offset: number = 0;
    public Limit: number = 25;
    constructor() {}
}