import { Injectable } from '@angular/core';
import {Http, RequestOptions, ConnectionBackend, RequestOptionsArgs, Response, Request, Headers} from "@angular/http";
import {Observable} from "rxjs";
import {Router} from "@angular/router";
import {AuthService} from "../../authentication/auth.service";
import {PubSubService} from "./pub-sub.service";
import {HttpInterceptor} from "@angular/common/http";
import {HttpRequest} from "@angular/common/http";
import {HttpHandler} from "@angular/common/http";
import {HttpEvent} from "@angular/common/http";
/**
 * http://www.adonespitogo.com/articles/angular-2-extending-http-provider/
 */
@Injectable()
export class JwtHttpService implements HttpInterceptor {
  public authService: AuthService;
  public pubsub: PubSubService;
  public router: Router;
  constructor(
      router: Router,
      authService: AuthService,
      pubsub: PubSubService) {
    this.router = router;
    this.authService = authService;
    this.pubsub = pubsub;
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${localStorage.getItem('auth_token')}`
      }
    });

    return next.handle(request);
  }
}
