// DTO matching API response - excludes CreatedAt/UpdatedAt (internal audit fields)
export interface VideoGame {
  id: number;
  title: string;
  genre: string | null;
  releaseDate: Date | null;
  publisher: string | null;
  rating: number | null;
  price: number | null;
  description: string | null;
}
