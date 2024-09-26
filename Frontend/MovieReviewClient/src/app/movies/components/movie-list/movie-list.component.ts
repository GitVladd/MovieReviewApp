// movie-list.component.ts (в папке movies/components)
import { Component, OnInit } from '@angular/core';
import { MovieService } from 'app/core/services/movie.service';

@Component({
  selector: 'app-movie-list',
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.css'],
})
export class MovieListComponent implements OnInit {
  movies: any[] = [];
  currentPage = 1;
  totalPages = 10;
  pageSize = 5;

  constructor(private movieService: MovieService) {}

  ngOnInit(): void {
    this.loadMovies();
  }

  loadMovies(): void {
    this.movieService.getMovies(this.currentPage, this.pageSize).subscribe((data) => {
      this.movies = data.results; // Предположим, что API возвращает массив фильмов в "results"
      this.totalPages = Math.ceil(data.totalCount / this.pageSize); // Предположим, что API возвращает общее количество фильмов
    });
  }

  changePage(page: number): void {
    if (page > 0 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadMovies();
    }
  }
}
