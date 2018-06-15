import { Directive, EventEmitter, HostListener, Output } from '@angular/core';

@Directive({
  selector: '[hwinfTrackscroll]'
})
export class TrackscrollDirective {
    @Output() scrolledToBottom = new EventEmitter();
    @HostListener("window:scroll", ['$event'])
    onWindowsScroll(ev: Event) {
        if ((window.innerHeight + window.scrollY) >= document.body.scrollHeight - 1) {
            this.scrolledToBottom.emit();
        }
    }
    constructor() { }
}
