import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'toArray'
})
export class ToArrayPipe implements PipeTransform {

  transform(value: any[], args?: any): any {

    console.log(value);

    for (let item of value) {
      console.log(item.toString());
    }

    return value;
  }

}
