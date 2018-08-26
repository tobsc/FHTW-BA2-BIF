import { Injectable, ViewContainerRef } from '@angular/core';
import { Overlay } from 'ngx-modialog';
import { Modal } from 'ngx-modialog/plugins/bootstrap';
import { Subject, Observable } from "rxjs";
import { RequestEventEmitter, ResponseEventEmitter } from './emmiters';
import { Error } from "../models/error.model";

@Injectable()
export class ErrorHandlerService {

    public error = new Subject<Error>();

    constructor() {
    }

    public getError(): Observable<Error> {
        return this.error.asObservable();
    }

    public showError(error:Error): void {
        this.error.next(error);
    }
}
