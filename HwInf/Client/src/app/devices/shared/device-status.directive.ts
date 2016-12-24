import {Directive, Input, HostBinding, ElementRef, Renderer, AfterViewInit} from '@angular/core';

@Directive({
  selector: '[hwInfDeviceStatus]'
})
export class DeviceStatusDirective implements AfterViewInit {

  @Input() statusId;

  constructor(private elem: ElementRef, private renderer: Renderer) {}

  ngAfterViewInit() {
    this.renderer.setElementClass(
      this.elem.nativeElement,
      this.getClass(),
      true
    );
  }

  private getClass(): string {
    switch(this.statusId) {
      case 1: return 'label-success';
      case 2: return 'label-warning';
      case 3: return 'label-danger';
      default: return 'label-default';
    }
  }
}
