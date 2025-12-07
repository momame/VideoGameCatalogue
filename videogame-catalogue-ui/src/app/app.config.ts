import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app.routes';

//Registering" services so Angular knows how to provide them when requested
//When VideoGameService asks for HttpClient, Angular checks this config and provides it
// Without provideRouter(routes), navigation wouldn't work
// Without provideZoneChangeDetection, Angular wouldn't update UI after async operations

export const appConfig: ApplicationConfig = {
  providers: [

    //Zone.js tracks async operations (HTTP calls, setTimeout, etc.) and triggers Angular's change detection.
    // eventCoalescing: true - optimizes performance by batching multiple events into one change detection cycle.
    provideZoneChangeDetection({ eventCoalescing: true }),

    // Makes Angular Router available throughout the app.
    // Routes are defined in app.routes.ts and injected here.
    // Enables navigation between components (e.g., /games, /games/edit/1).
    provideRouter(routes), 
    provideHttpClient()  // Registers HttpClient with DI container
  ]
};
