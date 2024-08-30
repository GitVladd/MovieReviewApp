using AutoMapper;
using ReviewService.Dtos;
using ReviewService.Models;

namespace ReviewService.Profiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Review, ReviewGetDto>();
            CreateMap<ReviewGetDto, Review>();

            CreateMap<Review, ReviewCreateDto>();
            CreateMap<ReviewCreateDto, Review>();

            CreateMap<Review, ReviewUpdateDto>();
            CreateMap<ReviewUpdateDto, Review>();

        }
    }
}
