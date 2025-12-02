import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Assunto, AssuntoCreateDto, AssuntoUpdateDto, ApiResult } from '../models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AssuntoService {
  private apiUrl = `${environment.apiUrl}/assunto`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Assunto[]> {
    return this.http.get<ApiResult<Assunto[]>>(this.apiUrl).pipe(
      map(result => result?.resultData ?? []),
      catchError((error) => {
        if (error.status === 404) {
          return of([]);
        }
        return throwError(() => error);
      })
    );
  }

  getById(id: string): Observable<Assunto> {
    return this.http.get<ApiResult<Assunto>>(`${this.apiUrl}/${id}`).pipe(
      map(result => result.resultData)
    );
  }

  create(assunto: AssuntoCreateDto): Observable<Assunto> {
    return this.http.post<ApiResult<Assunto>>(this.apiUrl, assunto).pipe(
      map(result => result.resultData)
    );
  }

  update(id: string, assunto: AssuntoUpdateDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, assunto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
