// DTO for updating existing video game - matches CreateVideoGame
export interface UpdateVideoGame {
  title: string;
  genre: string | null;
  releaseDate: Date | null;
  publisher: string | null;
  rating: number | null;
  price: number | null;
  description: string | null;
}
