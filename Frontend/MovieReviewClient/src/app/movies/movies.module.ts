import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovieListComponent } from './components/movie-list/movie-list.component';
import { MovieCardComponent } from './components/movie-card/movie-card.component';



@NgModule({
  declarations: [MovieListComponent, MovieCardComponent],
  imports: [
    CommonModule
  ]
})
export class MoviesModule { }
