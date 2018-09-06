import { Pipe, PipeTransform } from '@angular/core';
import {Field} from "../../../shared/models/field.model";

@Pipe({
  name: 'fieldsToArrayPipe'
})
export class FieldsToArrayPipe implements PipeTransform {

  transform(fields: Field[], args?: any): any {
    let names = [];
    for (let field of fields) {
      names.push(field.Name);
    }
    return names;
  }

}
