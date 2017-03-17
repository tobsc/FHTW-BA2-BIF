import { Injectable } from '@angular/core';
import { Http, RequestOptions, ConnectionBackend, RequestOptionsArgs, Response, Request, Headers } from "@angular/http";
import { Observable } from "rxjs";
import { Router } from "@angular/router";
import { PubSubService } from './pub-sub.service';

@Injectable()
export class FeedbackHttpService extends Http {

    private pubsub: PubSubService;

    constructor(
        backend: ConnectionBackend,
        defaultOptions: RequestOptions,
        router: Router,
        pubsub: PubSubService) {
        super(backend, defaultOptions);
        this.pubsub = pubsub;
    }

    request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
        return this.intercept(super.request(url, options));
    }

    intercept(observable: Observable<Response>): Observable<Response> {
        this.pubsub.beforeRequest.emit("beforeRequestEvent");
        //this will force the call to be made immediately..  
        observable.subscribe(
            null,
            null,
            () => this.pubsub.afterRequest.emit("afterRequestEvent")
          );
        return observable
    }

}
