
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieReviewApp.Common.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Dtos;
using UserService.Exceptions;
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
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					switch (error.Code)
					{
						case "PasswordRequiresDigit":
							throw new PasswordRequiresDigitException();
						case "PasswordRequiresLower":
							throw new PasswordRequiresLowercaseException();
						case "PasswordRequiresUpper":
							throw new PasswordRequiresUppercaseException();
						case "PasswordRequiresNonAlphanumeric":
							throw new PasswordRequiresNonAlphanumericException();
						case "PasswordTooShort":
							throw new PasswordTooShortException(_configuration.GetSection("PasswordRules").GetValue<int>("RequiredLength"));
						case "PasswordRequiresUniqueChars":
							throw new PasswordRequiresUniqueCharsException(_configuration.GetSection("PasswordRules").GetValue<int>("RequiredUniqueChars")); 
						case "DuplicateUserName":
							throw new UsernameMustBeUniqueException();
						case "DuplicateEmail":
							throw new UserRequiresUniqueEmailException();
						default:
							throw new Exception($"Registration failed: {error.Description}");
					}
				}
			}

			var roleAssigned = await AssignRoleToUserAsync(user.Id, "User");
			if (!roleAssigned)
			{
				await _userManager.DeleteAsync(user);
				throw new Exception("Failed to assign role. Please try again.");
			}

			return await GenerateJwtTokenAsync(user);
		}

		public async Task<string> LoginAsync(UserLoginDto userLoginDto)
		{
			var user = await _userManager.FindByNameAsync(userLoginDto.Username);

			if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
			{
				// General error message for invalid login attempt
				throw new Exception("Invalid login attempt. Please try again.");
			}

			return await GenerateJwtTokenAsync(user);
		}

		public async Task<bool> AssignRoleToUserAsync(Guid userId, string roleName)
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user == null)
			{
				throw new Exception("User not found.");
			}

			var result = await _userManager.AddToRoleAsync(user, roleName);
			return result.Succeeded;
		}

		//TO DO: Move to TokenService
		private async Task<string> GenerateJwtTokenAsync(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Convert.FromBase64String(_configuration["Jwt:Key"]);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName)
			};

			var userRoles = await _userManager.GetRolesAsync(user);
			foreach (var role in userRoles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
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
