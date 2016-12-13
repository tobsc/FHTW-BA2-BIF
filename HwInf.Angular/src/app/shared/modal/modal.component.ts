import { Component, OnInit } from '@angular/core';
import {Modal} from "./modal.model";

@Component({
  selector: 'hw-inf-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent {

  private modal: Modal = new Modal(null,null,null);

  public visible = false;
  private visibleAnimate = false;

  public show(modal: Modal): void {
    this.modal = modal;
    this.visible = true;
    setTimeout(() => this.visibleAnimate = true, 100);
  }

  public hide(): void {
    this.visibleAnimate = false;
    setTimeout(() => this.visible = false, 300);
  }
}
