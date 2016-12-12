export interface IDictionary<T> {
  add(key: string, value: T);
  containsKey(key: string): boolean;
  count(): number;
  keys(): string[];
  remove(key: string): T;
  values(): T[];
  get(key: string): T;
}
