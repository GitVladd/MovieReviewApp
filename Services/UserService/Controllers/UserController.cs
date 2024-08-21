using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Dtos;
using UserService.Exceptions;
using UserService.Service;

namespace UserService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly ILogger<UserController> _logger;
		private readonly IUserService _service;

		public UserController(
		   ILogger<UserController> logger,
		   IUserService service)
		{
			_logger = logger;
			_service = service;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var token = await _service.RegisterAsync(registerDto);
				return Ok(new { Token = token });
			}
			catch (PasswordRequiresDigitException ex)
			{
				_logger.LogError(ex, "Registration failed.");
				return BadRequest(ex.Message);
			}
			catch (PasswordRequiresLowercaseException ex)
			{
				_logger.LogError(ex, "Registration failed.");
				return BadRequest(ex.Message);
			}
			catch (PasswordRequiresUppercaseException ex)
			{
				_logger.LogError(ex, "Registration failed.");
				return BadRequest(ex.Message);
			}
			catch (PasswordRequiresNonAlphanumericException ex)
			{
				_logger.LogError(ex, "Registration failed.");
				return BadRequest(ex.Message);
			}
			catch (PasswordTooShortException ex)
			{
				_logger.LogError(ex, "Registration failed.");
				return BadRequest(ex.Message);
			}
			catch (PasswordRequiresUniqueCharsException ex)
			{
				_logger.LogError(ex, "Registration failed.");
				return BadRequest(ex.Message);
			}
			catch (UsernameMustBeUniqueException ex)
			{
				_logger.LogError(ex, "Registration failed.");
				return BadRequest(ex.Message);
			}
			catch (UserRequiresUniqueEmailException ex)
			{
				_logger.LogError(ex, "Registration failed.");
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Registration failed.");
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while registering. Please try again.");
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var token = await _service.LoginAsync(loginDto);
				return Ok(new { Token = token });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Login failed.");
				return Unauthorized("Invalid login attempt. Please try again.");
			}
		}
	}
}
