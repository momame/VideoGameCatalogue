import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { VideoGame } from '../../models/video-game.model';
import { VideoGameService } from '../../services/video-game';

/**
 * Game List Component - Browse page showing all video games.
 * Displays games in a table with edit/delete actions.
 * Uses Bootstrap for styling as required by assignment.
 */
@Component({
  selector: 'app-game-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './game-list.html',
  styleUrl: './game-list.css'
})
export class GameListComponent implements OnInit {
  games: VideoGame[] = [];
  loading = true;
  error: string | null = null;

  constructor(private videoGameService: VideoGameService) {}

  ngOnInit(): void {
    this.loadGames();
  }

  /**
   * Load all games from API.
   * Sets loading state and handles errors.
   */
  loadGames(): void {
    this.loading = true;
    this.error = null;
    
    this.videoGameService.getAll().subscribe({
      next: (games) => {
        this.games = games;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load games. Please check if the API is running.';
        this.loading = false;
        console.error('Error loading games:', err);
      }
    });
  }

  /**
   * Delete a game with confirmation.
   * Reloads list after successful deletion.
   */
  deleteGame(id: number, title: string): void {
    if (confirm(`Are you sure you want to delete "${title}"?`)) {
      this.videoGameService.delete(id).subscribe({
        next: () => {
          // Remove from local array without reloading from API
          this.games = this.games.filter(g => g.id !== id);
          alert('Game deleted successfully');
        },
        error: (err) => {
          alert('Failed to delete game');
          console.error('Error deleting game:', err);
        }
      });
    }
  }

  /**
   * Format date for display.
   * Returns formatted date string or 'N/A' if null.
   */
  formatDate(date: Date | null): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString();
  }

  /**
   * Format price for display.
   * Returns formatted currency or 'Free' if null/zero.
   */
  formatPrice(price: number | null): string {
    if (!price) return 'Free';
    return `$${price.toFixed(2)}`;
  }
}