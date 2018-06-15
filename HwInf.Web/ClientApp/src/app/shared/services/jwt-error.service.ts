import 'rxjs/add/operator/do';
import {HttpInterceptor} from "@angular/common/http";
import {AuthService} from "../../authentication/auth.service";
import {HttpRequest} from "@angular/common/http";
import {HttpHandler} from "@angular/common/http";
import {Observable} from "rxjs";
import {HttpEvent} from "@angular/common/http";
import {HttpResponse} from "@angular/common/http";
import {HttpErrorResponse} from "@angular/common/http";
import {Injectable} from "@angular/core";

@Injectable()
export class JwtErrorService implements HttpInterceptor {

  constructor(public authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request).do((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
      }
    }, (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401 || err.status === 403) {
          this.authService.logout();
        }
      }
    });
  }
}
