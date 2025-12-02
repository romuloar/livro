import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { TipoCompra, ApiResult } from '../models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TipoCompraService {
  private apiUrl = `${environment.apiUrl}/tipo-compra`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<TipoCompra[]> {
    return this.http.get<ApiResult<TipoCompra[]>>(this.apiUrl).pipe(
      map(result => result?.resultData ?? []),
      catchError((error) => {
        if (error.status === 404) {
          return of([]);
        }
        return throwError(() => error);
      })
    );
  }
}
