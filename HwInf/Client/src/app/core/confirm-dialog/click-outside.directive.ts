import {Directive, ElementRef, Output, EventEmitter, HostListener} from '@angular/core';

@Directive({
  selector: '[ClickOutside]'
})
export class ClickOutsideDirective {

  @Output()
  public clickOutside = new EventEmitter();

  constructor(private el: ElementRef) { }

  @HostListener('document:click', ['$event.target'])
  public onClick(targetElement) {
    const clickInside = this.el.nativeElement.contains(targetElement);
    if (!clickInside) {
      this.clickOutside.emit();
    }
  }

}
