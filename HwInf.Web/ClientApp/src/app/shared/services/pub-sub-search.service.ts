import { Injectable } from '@angular/core';
import { Overlay } from 'angular2-modal';
import { Modal } from 'angular2-modal/plugins/bootstrap';
import { Subject, Observable } from "rxjs";
import { RequestEventEmitter, ResponseEventEmitter } from './emmiters';

@Injectable()
export class PubSubSearchService {

 private searchText = new Subject<string>();

    constructor() {
    }

    public getSearchText(): Observable<string> {
        return this.searchText.asObservable();
    }

    public publishSearchText(searchText :string): void {
        this.searchText.next(searchText);
    }

}
