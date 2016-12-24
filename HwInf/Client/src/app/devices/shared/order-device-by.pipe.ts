import { Pipe, PipeTransform } from '@angular/core';
import {Device} from "./device.model";

@Pipe({
    name: 'orderDeviceBy'
})
export class OrderDeviceByPipe implements PipeTransform {

    transform(value: Device[], args?: string): any {
        if ( value == null ) {
            return;
        }
        let result: Device[] = value;
        this.sort(result, 'name');
        if (args === 'status') {
            this.sort(result, args);
        }
        return result;
    }

    private sort(value: Device[], args: string): void {
        value.sort((a: Device, b: Device) => {
            let x,y;
            if (args === 'name') {
                x = a.Name;
                y = b.Name;
            }
            else if (args === 'status') {
                x = a.StatusId.toString();
                y = b.StatusId.toString();
            }
            return this.compare(x,y);
        });
    }

    private compare(a: string, b: string): number {
        let x: string = a.toLowerCase();
        let y: string = b.toLowerCase();
        return (x < y) ? -1 : (x > y) ? 1 : 0;
    }

}
