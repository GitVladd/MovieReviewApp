
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MovieReviewApp.Common.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Service
{
	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		public UserService(UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
		{
			_userManager = userManager;
			_mapper = mapper;
			_configuration = configuration;
		}

		public async Task<string> RegisterAsync(UserRegisterDto userRegisterDto)
		{
			var user = _mapper.Map<User>(userRegisterDto);
			var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

			if (result.Succeeded)
			{
				return await GenerateJwtTokenAsync(user);
			}

			throw new Exception("Registration failed.");
		}

		public async Task<string> LoginAsync(UserLoginDto userLoginDto)
		{
			var user = await _userManager.FindByNameAsync(userLoginDto.Username);
			if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
			{
				throw new Exception("Invalid login attempt.");
			}

			return await GenerateJwtTokenAsync(user);
		}
		//TO DO: Move to TokenService
		private async Task<string> GenerateJwtTokenAsync(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
					new Claim(ClaimTypes.Name, user.UserName)
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				Issuer = _configuration["Jwt:Issuer"],
				Audience = _configuration["Jwt:Audience"],
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
