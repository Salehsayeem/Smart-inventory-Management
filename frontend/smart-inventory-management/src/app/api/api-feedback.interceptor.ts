import { Injectable, inject } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse, HttpErrorResponse
} from '@angular/common/http';
import { Observable, tap, catchError } from 'rxjs';
import { SnackbarService } from '../components/snackbar/snackbar.service';

@Injectable()
export class ApiFeedbackInterceptor implements HttpInterceptor {
  private snackbar = inject(SnackbarService);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      tap(event => {
        if (event instanceof HttpResponse) {
          // Show success for status 200-299
          if (event.body && event.body.message) {
            const status = event.body.statusCode || event.status;
            if (status >= 200 && status < 300) {
              this.snackbar.show(event.body.message, 'success');
            } else {
              this.snackbar.show(event.body.message, 'error');
            }
          }
        }
      }),
      catchError((error: HttpErrorResponse) => {
        this.snackbar.show(error.error?.message || 'Request failed', 'error');
        throw error;
      })
    );
  }
}
