import { Pipe, PipeTransform } from '@angular/core';
import {FieldGroup} from "../models/fieldgroup.model";

@Pipe({
  name: 'excludeFieldGroup'
})
export class ExcludeFieldGroupPipe implements PipeTransform {

  transform(value: FieldGroup[], args?: any): any {
    return value.filter(i => args.indexOf(i.Slug));
  }

}
