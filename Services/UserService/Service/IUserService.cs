﻿using UserService.Dtos;

namespace UserService.Service
{
	public interface IUserService
	{
		Task<string> RegisterAsync(UserRegisterDto userRegisterDto);
		Task<string> LoginAsync(UserLoginDto userLoginDto);
	}
}