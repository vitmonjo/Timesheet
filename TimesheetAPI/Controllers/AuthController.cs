using Microsoft.AspNetCore.Mvc;
using TimesheetAPI.Authentication;
using TimesheetAPI.DTOs;
using TimesheetAPI.Authentication;

namespace TimesheetAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                var response = _authService.Register(registerDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var response = _authService.Login(loginDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
