using AutoMapper;
using MovieService.Dtos.CategoryDto;
using MovieService.Models;

namespace MovieService.Profiles
{
	public class CategoryProfile : Profile
	{
		public CategoryProfile()
		{
			CreateMap<Category, CategoryGetDto>();
			CreateMap<CategoryGetDto, Category>();

			CreateMap<Category, CategoryCreateDto>();
			CreateMap<CategoryCreateDto, Category>();
		}
	}
}
