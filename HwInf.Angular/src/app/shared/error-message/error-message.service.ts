import {Injectable} from '@angular/core';
import {Subject, Observable} from "rxjs";
import {Modal} from "../modal/modal.model";


@Injectable()
export class ErrorMessageService {

  private text = new Subject<Modal>();

  public getMessage(): Observable<Modal> {
    return this.text.asObservable();
  }

  public showErrorMessage(modal: Modal): void {
    this.text.next(modal);
  }

}
