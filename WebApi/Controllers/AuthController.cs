using Business.Authentication;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromForm]RegisterAuthDto registerDto)
        {
            var result = _authService.Register(registerDto);

            if (result.Success) return Ok(result);

            return BadRequest(result.Message);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginAuthDto loginDto)
        {
            var result = _authService.Login(loginDto);
            if (result.Success) return Ok(result);

            return BadRequest(result.Message);
        }
    }
}
