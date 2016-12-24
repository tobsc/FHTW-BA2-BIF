import {Directive, HostBinding, HostListener} from '@angular/core';

@Directive({
  selector: '[bs3Dropdown]'
})
export class DropdownDirective {

  private isOpen: boolean = false;

  constructor() { }

  @HostBinding('class.open')
  get opened(): boolean {
    return this.isOpen;
  }

  @HostListener('click')
  public pen(): void {
    this.isOpen = !this.isOpen;
  }

  @HostListener('mouseleave')
  public close(): void {
    this.isOpen = false;
  }
}
