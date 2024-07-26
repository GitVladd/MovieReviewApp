using AutoMapper;
using MovieService.Dtos.ContentTypeDto;
using MovieService.Models;

namespace MovieService.Profiles
{
	public class ContentTypeProfile : Profile
	{
		public ContentTypeProfile()
		{
			CreateMap<ContentType, ContentTypeGetDto>();
			CreateMap<ContentTypeGetDto, ContentType>();

			CreateMap<ContentType, ContentTypeCreateDto>();
			CreateMap<ContentTypeCreateDto, ContentType>();
		}
	}
}
