import { Injectable, Output } from '@angular/core';
import {
  HttpInterceptor,
  HttpEvent,
  HttpHandler,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { GlobalStoreService } from './global-store.service';

@Injectable()
export class CustomInterceptor implements HttpInterceptor {
  constructor(private store: GlobalStoreService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let reqWithHeaders = req.clone({
      setHeaders: { Authorization: 'bearer ' + this.store.getToken() },
    });
    return next.handle(reqWithHeaders);
  }
}
