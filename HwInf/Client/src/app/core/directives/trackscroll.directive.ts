import { Directive, EventEmitter } from '@angular/core';
import { HostListener, Output } from "@angular/core/src/metadata/directives";

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
