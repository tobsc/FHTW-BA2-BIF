import { Field } from "./field.model";

export class Group {
    constructor(
        public GroupId: number,
        public Name: string,
        public Label: string,
        public Field: Field[]
    ) { }
}