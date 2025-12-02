import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Livro, LivroCreateDto, LivroUpdateDto, ApiResult } from '../models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LivroService {
  private apiUrl = `${environment.apiUrl}/livro`;

  constructor(private http: HttpClient) { }

  getAll(): Observable<Livro[]> {
    return this.http.get<ApiResult<Livro[]>>(this.apiUrl).pipe(
      map(result => result?.resultData ?? []),
      catchError((error) => {
        if (error.status === 404) {
          return of([]);
        }
        return throwError(() => error);
      })
    );
  }

  getById(id: string): Observable<Livro> {
    return this.http.get<ApiResult<Livro>>(`${this.apiUrl}/${id}`).pipe(
      map(result => result.resultData)
    );
  }

  create(livro: LivroCreateDto): Observable<Livro> {
    return this.http.post<ApiResult<Livro>>(this.apiUrl, livro).pipe(
      map(result => result.resultData)
    );
  }

  update(id: string, livro: LivroUpdateDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, livro);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
