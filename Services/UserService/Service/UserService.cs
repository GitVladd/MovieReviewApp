using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MovieReviewApp.Common.Exceptions;
using UserService.Dtos;
using UserService.Exceptions;
using UserService.Models;
namespace UserService.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IJwtGeneratorService _jwtGeneratorService;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, IMapper mapper, IConfiguration configuration, IJwtGeneratorService jwtGeneratorService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _jwtGeneratorService = jwtGeneratorService;
        }

        public async Task<string> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var user = _mapper.Map<User>(userRegisterDto);

            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
            try
            {
                HandleUserCreationResult(result);

                var roleName = "User";

                var roleAssigned = await AssignRoleToUserAsync(user.Id, roleName);

                if (!roleAssigned)
                    throw new Exception($"Failed to assign role: {roleName}");

            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(user);
                throw;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            return await _jwtGeneratorService.GenerateJwtTokenAsync(user, userRoles);


        }

        public async Task<string> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userLoginDto.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return await _jwtGeneratorService.GenerateJwtTokenAsync(user, userRoles);
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                throw new EntityNotFoundException($"Role {roleName} does not exist");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        private void HandleUserCreationResult(IdentityResult result)
        {
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
        }
    }
}
