using LanguageCards.API.ApiModels.Auth;
using LanguageCards.API.Services.Abstractions;
using LanguageCards.Application.Commands.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LanguageCards.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ISender _sender;

        public AuthController(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest authenticationRequest)
        {
            var response = await _authenticationManager.GenerateTokenAsync(authenticationRequest, HttpContext.RequestAborted);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            var command = new RegisterUserCommand(
                registrationRequest.Email,
                registrationRequest.Password,
                registrationRequest.FirstName,
                registrationRequest.LastName);

            await _sender.Send(command, HttpContext.RequestAborted);

            return Ok();
        }
    }
}
