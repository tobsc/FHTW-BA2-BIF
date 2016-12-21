import { Injectable } from '@angular/core';
import {Http, RequestOptions, ConnectionBackend, RequestOptionsArgs, Response, Request, Headers} from "@angular/http";
import {Observable} from "rxjs";
import {ErrorMessageService} from "./error-message/error-message.service";
import {Modal} from "./modal/modal.model";
import {Router} from "@angular/router";
import {AuthService} from "../authentication/auth.service";
/**
 * http://www.adonespitogo.com/articles/angular-2-extending-http-provider/
 */
@Injectable()
export class JwtHttpService extends Http {

  private authService: AuthService;
  private router: Router;
  constructor(backend: ConnectionBackend, defaultOptions: RequestOptions, authService: AuthService, router: Router) {
    let token = localStorage.getItem('auth_token');
    defaultOptions.headers.set('Authorization', 'Bearer ${token}');
    super(backend, defaultOptions);
    this.authService = authService;
    this.router = router;
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
    return super.request(url, options).catch(this.catchAuthError(this));
  }

  private catchAuthError (self: JwtHttpService) {
    // we have to pass HttpService's own instance here as `self`
    return (res: Response) => {

      if (res.status === 401 || res.status === 403) {
        // if not authenticated
        this.authService.logout();
        this.router.navigate(['/login']);
      }
      return Observable.throw(res);
    };
  }
}
