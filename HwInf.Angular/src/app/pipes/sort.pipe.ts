import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sort'
})
export class SortPipe implements PipeTransform {

  transform(value: string[], args?: string): any {

      let result: string [] = value;

    result.sort((a: string, b: string) => {
      return this.ascCompare(a,b);
    });

      console.log(result);

      return result;
  }

  private ascCompare(a: string, b: string): number {
    let x: string = a.toLowerCase();
    let y: string = b.toLowerCase();
    return (x < y) ? -1 : (x > y) ? 1 : 0;
  }

  private descCompare(a: string, b: string): number {
    let x: string = a.toLowerCase();
    let y: string = b.toLowerCase();
    return (x > y) ? -1 : (x < y) ? 1 : 0;
  }
}
