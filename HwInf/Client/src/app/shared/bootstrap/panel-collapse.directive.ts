import {Directive, ElementRef, HostListener, Renderer, AfterViewInit, HostBinding} from '@angular/core';

@Directive({
  selector: '[hwInfPanelCollapse]'
})
export class PanelCollapseDirective implements AfterViewInit {

  private isCollapsed: boolean = true;

  constructor(private elem: ElementRef, private renderer: Renderer) {}

  ngAfterViewInit() {
    this.renderer.setElementClass(
      this.elem.nativeElement.nextElementSibling,
      'collapse',
      this.isCollapsed
    );

    this.renderer.setElementStyle(
      this.elem.nativeElement,
      'border-color',
      this.getBorderColor()
    );
  }

  @HostListener('click') toggleClass() {

    this.isCollapsed = !this.isCollapsed;

    this.renderer.setElementStyle(
      this.elem.nativeElement,
      'border-color',
      this.getBorderColor()
    );

    this.renderer.setElementClass(
      this.elem.nativeElement.nextElementSibling,
      'collapse',
      this.isCollapsed
    );
  }

  getBorderColor(): string {
    return (this.isCollapsed) ? 'transparent' : '#ddd';
  }
}
