import {IDictionary} from "./IDictionary";
export class Dictionary<T> implements IDictionary<T> {

  private items: { [key: string]: T } = {};
  private _count: number = 0;

  add(key: string, value: T) {
    this.items[key] = value;
    this._count++;
  }

  containsKey(key: string): boolean {
    return this.items.hasOwnProperty(key);
  }

  count(): number {
    return this._count;
  }

  keys(): string[] {
    var keySet: string[] = [];
    for (var prop in this.items) {
      if (this.items.hasOwnProperty(prop)) {
        keySet.push(prop);
      }
    }
    return keySet;
  }

  remove(key: string): T {
    var val = this.items[key];
    delete this.items[key];
    this._count--;
    return val;
  }

  values(): T[] {
    var values: T[] = [];
    for (var prop in this.items) {
      if (this.items.hasOwnProperty(prop)) {
        values.push(this.items[prop]);
      }
    }
    return values;
  }

  get(key: string): T {
    return this.items[key];
  }
}
