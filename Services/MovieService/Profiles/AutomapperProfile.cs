using AutoMapper;
using MovieService.Dtos.CategoryDto;
using MovieService.Dtos.ContentTypeDto;
using MovieService.Dtos.MovieDto;
using MovieService.Models;

namespace MovieService.Profiles
{
	public class AutomapperProfile : Profile
	{
		public AutomapperProfile()
		{
			CreateMap<Movie, MovieGetDto>();
			CreateMap<MovieGetDto, Movie>();

			CreateMap<Movie, MovieCreateDto>();
			CreateMap<MovieCreateDto, Movie>();

			CreateMap<Movie, MovieUpdateDto>();
			CreateMap<MovieUpdateDto, Movie>();


			CreateMap<ContentType, ContentTypeGetDto>();
			CreateMap<ContentTypeGetDto, ContentType>();

			CreateMap<ContentType, ContentTypeCreateDto>();
			CreateMap<ContentTypeCreateDto, ContentType>();


			CreateMap<Category, CategoryGetDto>();
			CreateMap<CategoryGetDto, Category>();

			CreateMap<Category, CategoryCreateDto>();
			CreateMap<CategoryCreateDto, Category>();
		}
	}
}
