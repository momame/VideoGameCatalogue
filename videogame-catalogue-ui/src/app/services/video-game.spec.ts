import { TestBed } from '@angular/core/testing';

import { VideoGame } from './video-game';

describe('VideoGame', () => {
  let service: VideoGame;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VideoGame);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
