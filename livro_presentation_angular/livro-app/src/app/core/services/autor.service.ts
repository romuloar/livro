import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Autor, AutorCreateDto, AutorUpdateDto, ApiResult } from '../models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AutorService {
  private apiUrl = `${environment.apiUrl}/autor`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Autor[]> {
    return this.http.get<ApiResult<Autor[]>>(this.apiUrl).pipe(
      map(result => result?.resultData ?? []),
      catchError((error) => {
        if (error.status === 404) {
          return of([]);
        }
        return throwError(() => error);
      })
    );
  }

  getById(id: string): Observable<Autor> {
    return this.http.get<ApiResult<Autor>>(`${this.apiUrl}/${id}`).pipe(
      map(result => result.resultData)
    );
  }

  create(autor: AutorCreateDto): Observable<Autor> {
    return this.http.post<ApiResult<Autor>>(this.apiUrl, autor).pipe(
      map(result => result.resultData)
    );
  }

  update(id: string, autor: AutorUpdateDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, autor);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
