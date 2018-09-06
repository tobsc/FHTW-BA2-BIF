import { Field } from "./field.model";

export class FieldGroup {
    public GroupId: number;
    public Name: string;
    public Slug: string;
    public IsActive: boolean;
    public Fields: Field[];
    public IsCountable: boolean;
    constructor() { }
}