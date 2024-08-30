using AutoMapper;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Profiles
{
	public class AutomapperProfile : Profile
	{
		public AutomapperProfile()
		{
			CreateMap<UserLoginDto, User>();
			CreateMap<UserRegisterDto, User>();

		}
	}
}
