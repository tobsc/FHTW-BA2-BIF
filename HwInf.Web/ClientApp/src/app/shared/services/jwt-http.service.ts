import { Injectable } from '@angular/core';
import {Http, RequestOptions, ConnectionBackend, RequestOptionsArgs, Response, Request, Headers} from "@angular/http";
import {Observable} from "rxjs";
import {Router} from "@angular/router";
import {AuthService} from "../../authentication/auth.service";
import {PubSubService} from "./pub-sub.service";
/**
 * http://www.adonespitogo.com/articles/angular-2-extending-http-provider/
 */
@Injectable()
export class JwtHttpService extends Http {
  private authService: AuthService;
  private pubsub: PubSubService;
  private router: Router;
  constructor(
      backend: ConnectionBackend,
      defaultOptions: RequestOptions,
      router: Router,
      authService: AuthService,
      pubsub: PubSubService) {
    let token = localStorage.getItem('auth_token');
    defaultOptions.headers.set('Authorization', 'Bearer ${token}');
    super(backend, defaultOptions);
    this.router = router;
    this.authService = authService;
    this.pubsub = pubsub;
  }

  request(url: string|Request, options?: RequestOptionsArgs): Observable<Response> {
    let token = localStorage.getItem('auth_token');
    if (typeof url === 'string') { // meaning we have to add the token to the options, not in url
      if (!options) {
        // let's make option object
        options = {headers: new Headers()};
      }
      options.headers.set('Authorization', `Bearer ${token}`);
    } else {
      // we have to add the token to the url object
      url.headers.set('Authorization', `Bearer ${token}`);
    }
    return this.intercept(super.request(url, options)).catch(this.catchAuthError(this));
  }

  intercept(observable: Observable<Response>): Observable<Response> {
    this.pubsub.beforeRequest.emit("beforeRequestEvent");
    return observable.do(() => this.pubsub.afterRequest.emit("afterRequestEvent"));
  }


  private catchAuthError (self: JwtHttpService) {
    // we have to pass HttpService's own instance here as `self`
    return (res: Response) => {

      this.pubsub.afterRequest.emit("afterRequestEvent");

      if (res.status === 401 || res.status === 403) {
        // if not authenticated
        this.authService.logout();
        //this.router.navigate(['/login']);
      }
      return Observable.throw(res);
    };
  }
}
