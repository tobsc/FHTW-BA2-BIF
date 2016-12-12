import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'hw-inf-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent {

  private modalText: string;

  public visible = false;
  private visibleAnimate = false;

  public show(text: string = null): void {
    this.modalText = text;
    this.visible = true;
    setTimeout(() => this.visibleAnimate = true, 100);
  }

  public hide(): void {
    this.visibleAnimate = false;
    setTimeout(() => this.visible = false, 300);
  }
}
