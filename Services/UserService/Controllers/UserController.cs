using Microsoft.AspNetCore.Mvc;
using UserService.Dtos;
using UserService.Service;

namespace UserService.Controllers
{
	public class UserController : ControllerBase
	{
		private readonly ILogger<UserController> _logger;
		private readonly IUserService _service;

		[HttpPost]
		public Task<ActionResult> Register([FromBody] UserRegisterDto createDto)
		{
		}

		[HttpPost]
		public Task<ActionResult> Login([FromBody] UserLoginDto createDto)
		{
		}
	}
}
