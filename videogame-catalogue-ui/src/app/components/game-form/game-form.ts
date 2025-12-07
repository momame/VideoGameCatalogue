import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { VideoGameService } from '../../services/video-game';
import { CreateVideoGame } from '../../models/create-video-game.model';
import { UpdateVideoGame } from '../../models/update-video-game.model';

/**
 * Game Form Component - Handles both Create and Edit operations.
 * Uses Reactive Forms with validation.
 * Route parameter determines if creating new or editing existing game.
 */
@Component({
  selector: 'app-game-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './game-form.html',
  styleUrl: './game-form.css'
})
export class GameFormComponent implements OnInit {
  gameForm!: FormGroup;
  isEditMode = false;
  gameId: number | null = null;
  loading = false;
  error: string | null = null;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private videoGameService: VideoGameService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    // Initialize form with validation rules
    this.initializeForm();

    // Check if editing existing game (id in route parameter)
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.gameId = +params['id'];  // Convert string to number
        this.loadGame(this.gameId);
      }
    });
  }

  /**
   * Initialize reactive form with validation.
   * Title is required, other fields optional but validated if provided.
   */
  initializeForm(): void {
    this.gameForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      genre: ['', Validators.maxLength(50)],
      releaseDate: [''],
      publisher: ['', Validators.maxLength(100)],
      rating: ['', [Validators.min(0), Validators.max(10)]],
      price: ['', [Validators.min(0), Validators.max(9999.99)]],
      description: ['', Validators.maxLength(1000)]
    });
  }

  /**
   * Load existing game data for editing.
   * Populates form with current values from API.
   */
  loadGame(id: number): void {
    this.loading = true;
    this.videoGameService.getById(id).subscribe({
      next: (game) => {
        // Format date for input field (YYYY-MM-DD)
        const formattedDate = game.releaseDate 
          ? new Date(game.releaseDate).toISOString().split('T')[0] 
          : '';

        this.gameForm.patchValue({
          title: game.title,
          genre: game.genre,
          releaseDate: formattedDate,
          publisher: game.publisher,
          rating: game.rating,
          price: game.price,
          description: game.description
        });
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load game data';
        this.loading = false;
        console.error('Error loading game:', err);
      }
    });
  }

  /**
   * Form submission handler.
   * Creates new game or updates existing based on mode.
   */
  onSubmit(): void {
    this.submitted = true;

    // Stop if form is invalid
    if (this.gameForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = null;

    // Get form values
    const formValue = this.gameForm.value;

    // Convert empty strings to null for optional fields
    const gameData = {
      title: formValue.title,
      genre: formValue.genre || null,
      releaseDate: formValue.releaseDate || null,
      publisher: formValue.publisher || null,
      rating: formValue.rating ? +formValue.rating : null,  // Convert to number
      price: formValue.price ? +formValue.price : null,     // Convert to number
      description: formValue.description || null
    };

    if (this.isEditMode && this.gameId) {
      // Update existing game
      this.videoGameService.update(this.gameId, gameData as UpdateVideoGame).subscribe({
        next: () => {
          this.router.navigate(['/games']);
        },
        error: (err) => {
          this.error = 'Failed to update game';
          this.loading = false;
          console.error('Error updating game:', err);
        }
      });
    } else {
      // Create new game
      this.videoGameService.create(gameData as CreateVideoGame).subscribe({
        next: () => {
          this.router.navigate(['/games']);
        },
        error: (err) => {
          this.error = 'Failed to create game';
          this.loading = false;
          console.error('Error creating game:', err);
        }
      });
    }
  }

  /**
   * Cancel and return to list without saving.
   */
  onCancel(): void {
    this.router.navigate(['/games']);
  }

  /**
   * Helper to check if field has validation error and was touched.
   */
  hasError(fieldName: string): boolean {
    const field = this.gameForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched || this.submitted));
  }

  /**
   * Get error message for a field.
   */
  getErrorMessage(fieldName: string): string {
    const field = this.gameForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) return `${fieldName} is required`;
    if (field.errors['maxlength']) return `${fieldName} is too long`;
    if (field.errors['min']) return `${fieldName} must be at least ${field.errors['min'].min}`;
    if (field.errors['max']) return `${fieldName} cannot exceed ${field.errors['max'].max}`;

    return 'Invalid value';
  }
}