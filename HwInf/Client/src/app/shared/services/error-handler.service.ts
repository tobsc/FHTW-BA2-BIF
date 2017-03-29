import { Injectable, ViewContainerRef } from '@angular/core';
import { Overlay } from 'angular2-modal';
import { Modal } from 'angular2-modal/plugins/bootstrap';
import { Subject, Observable } from "rxjs";
import { RequestEventEmitter, ResponseEventEmitter } from './emmiters';
import { Error } from "../models/error.model";

@Injectable()
export class ErrorHandlerService {

    private error = new Subject<Error>();

    constructor() {
    }

    public getError(): Observable<Error> {
        return this.error.asObservable();
    }

    public showError(error:Error): void {
        this.error.next(error);
    }
}
