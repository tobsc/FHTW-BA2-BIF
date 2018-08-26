import { Directive, Input, HostBinding, ElementRef, Renderer, AfterViewInit } from '@angular/core';

@Directive({
    selector: '[hwInfDeviceStatus]'
})
export class DevicesStatusDirective implements AfterViewInit {

    @Input() statusId;

    constructor(public elem: ElementRef, public renderer: Renderer) { }

    ngAfterViewInit() {
        this.renderer.setElementClass(
            this.elem.nativeElement,
            this.getClass(),
            true
        );
    }

    public getClass(): string {
        switch (this.statusId) {
            case 1: return 'label-success';
            case 2: return 'label-warning';
            case 3: return 'label-danger';
            default: return 'label-default';
        }
    }
}
