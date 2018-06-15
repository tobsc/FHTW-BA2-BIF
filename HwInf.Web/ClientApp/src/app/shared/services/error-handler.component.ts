import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { Subscription, Observable } from "rxjs";
import { Overlay } from 'ngx-modialog';
import { Modal } from 'ngx-modialog/plugins/bootstrap';
import { ErrorHandlerService } from "./error-handler.service";
import { Error } from "../models/error.model";



@Component({
    selector: 'hwinf-error-handler',
    template: '<div></div>'
})
export class ErrorHandlerComponent implements OnInit {
    
    constructor(
        private errorHandlerService: ErrorHandlerService,
        overlay: Overlay,
        vcRef: ViewContainerRef,
        public modal: Modal
    ) {
    }
    

    ngOnInit() {
        this.errorHandlerService.getError().subscribe(
            (data: Error) => {
                this.show(data)
            }
        );
    }

    show(msg: Error):void {
        this.modal.alert()
            .showClose(true)
            .keyboard(27)
            .title(msg.header)
            .body(msg.body)
            .open();

    }

}
