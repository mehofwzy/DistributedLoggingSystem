using DLS.Server.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = DLS.Server.Models.LoginRequest;



namespace DLS.Server.Controllers
{
    [Route("v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validate user credentials here (e.g., check against a database)
            if (request.Username == "admin" && request.Password == "1234") // this only for test and will be with actual validation in real app
            {
                var token = _authService.GenerateToken(request.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
