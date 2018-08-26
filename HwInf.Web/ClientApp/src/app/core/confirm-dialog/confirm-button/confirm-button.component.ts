import {Component, OnInit, Output, EventEmitter, ViewChild, ElementRef, Input} from '@angular/core';


@Component({
  selector: 'hwinf-confirm-button',
  templateUrl: './confirm-button.component.html',
  styleUrls: ['./confirm-button.component.scss']
})
export class ConfirmButtonComponent implements OnInit {

  @ViewChild('pop') pop: any;
  @Output('action') action = new EventEmitter();
  @Input('popOverTitle') title: string;

  constructor(public el: ElementRef) { }

  ngOnInit() {
  }

  onAction() {
    this.action.emit();
    this.pop.hide();
  }

  onDismiss() {
    this.pop.hide();
  }
}
