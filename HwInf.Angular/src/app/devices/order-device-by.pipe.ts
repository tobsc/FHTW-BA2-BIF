import { Pipe, PipeTransform } from '@angular/core';
import {Device} from "./device.class";

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
                x = a.Description;
                y = b.Description;
            }
            else if (args === 'status') {
                x = a.StatusId;
                y = b.StatusId;
            }
            return this.compare(x,y);
        });
    }

    private compare(x: string, y: string): number {
        return (x < y) ? -1 : (x > y) ? 1 : 0;
    }

}
