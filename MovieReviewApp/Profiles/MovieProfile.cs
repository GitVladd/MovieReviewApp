using AutoMapper;
using MovieService.Dtos.MovieDto;
using MovieService.Models;

namespace MovieService.Profiles
{
	public class MovieProfile : Profile
	{
		public MovieProfile()
		{
			CreateMap<Movie, MovieGetDto>();
			CreateMap<MovieGetDto, Movie>();

			CreateMap<Movie, MovieCreateDto>();
			CreateMap<MovieCreateDto, Movie>();

			CreateMap<Movie, MovieUpdateDto>();
			CreateMap<MovieUpdateDto, Movie>();
		}
	}
}
