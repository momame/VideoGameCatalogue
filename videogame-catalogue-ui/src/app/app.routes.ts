import { Routes } from '@angular/router';
import { GameListComponent } from './components/game-list/game-list';
import { GameFormComponent } from './components/game-form/game-form';

export const routes: Routes = [
  { path: '', redirectTo: '/games', pathMatch: 'full' },
  { path: 'games', component: GameListComponent },
  { path: 'games/new', component: GameFormComponent },
  { path: 'games/edit/:id', component: GameFormComponent },
  { path: '**', redirectTo: '/games' }
];