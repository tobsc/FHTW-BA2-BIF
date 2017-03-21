import { Field } from "./field.model";

export class FieldGroup {
    constructor(
        public GroupId: number,
        public Name: string,
        public Slug: string,
        public Fields: Field[]
    ) { }
}