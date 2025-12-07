import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { VideoGame } from '../models/video-game.model';
import { CreateVideoGame } from '../models/create-video-game.model';
import { UpdateVideoGame } from '../models/update-video-game.model';
import { environment } from '../../environment.development';

/**
 * Service for video game CRUD operations.
 * Handles all HTTP communication with the backend API.
 * Uses dependency injection to get HttpClient.
 */
@Injectable({
  providedIn: 'root'  // Singleton service available app-wide
})
export class VideoGameService {
  // API base URL from environment configuration
  private apiUrl = `${environment.apiUrl}/videogames`;

  /**
   * Constructor with HttpClient injection.
   * HttpClient is provided by provideHttpClient() in app.config.ts
   */
  constructor(private http: HttpClient) { }

  /**
   * GET all video games from API.
   * Returns Observable that components can subscribe to.
   */
  getAll(): Observable<VideoGame[]> {
    return this.http.get<VideoGame[]>(this.apiUrl)
      .pipe(
        catchError(this.handleError)  // Centralized error handling
      );
  }

  /**
   * GET single video game by ID.
   * Returns null if not found (API returns 404).
   */
  getById(id: number): Observable<VideoGame> {
    return this.http.get<VideoGame>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * POST - Create new video game.
   * API returns created game with generated ID and 201 status.
   */
  create(game: CreateVideoGame): Observable<VideoGame> {
    return this.http.post<VideoGame>(this.apiUrl, game)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * PUT - Update existing video game.
   * API returns updated game with 200 status, or 404 if not found.
   */
  update(id: number, game: UpdateVideoGame): Observable<VideoGame> {
    return this.http.put<VideoGame>(`${this.apiUrl}/${id}`, game)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * DELETE - Remove video game by ID.
   * API returns 204 No Content on success, 404 if not found.
   */
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Centralized error handler for HTTP errors.
   * Logs error and returns user-friendly message.
   * In production, would send to logging service.
   */
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Backend returned unsuccessful response code
      errorMessage = `Server returned code ${error.status}, error message is: ${error.message}`;
    }

    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
