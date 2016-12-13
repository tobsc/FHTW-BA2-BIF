import {Component, OnInit, ViewChild} from '@angular/core';
import {ModalComponent} from "../modal/modal.component";
import {ErrorMessageService} from "./error-message.service";
import {Modal} from "../modal/modal.model";

@Component({
  selector: 'hw-inf-error-message',
  templateUrl: './error-message.component.html',
  styleUrls: ['./error-message.component.scss']
})
export class ErrorMessageComponent implements OnInit {

  @ViewChild(ModalComponent) modal: ModalComponent;

  constructor(private errorMessageService: ErrorMessageService) { }

  ngOnInit() {

    this.errorMessageService.getMessage().subscribe(
      (response: Modal) => {
          this.modal.show(response);
      }
    );

  }


}
