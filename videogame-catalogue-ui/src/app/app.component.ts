import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VideoGameService } from './services/video-game';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'videogame-catalogue-ui';

  constructor(private videoGameService: VideoGameService) { }

  ngOnInit() {
    // Test API call - check browser console
    this.videoGameService.getAll().subscribe({
      next: (games) => console.log('Games from API:', games),
      error: (err) => console.error('Error:', err)
    });
  }
}
