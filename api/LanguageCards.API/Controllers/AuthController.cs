using LanguageCards.API.ApiModels.Auth;
using LanguageCards.API.Services.Abstractions;
using LanguageCards.Application.Commands.Authorization;
using LanguageCards.Application.Services.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LanguageCards.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IGoogleAuthenticationService _googleAuthenticationService;
        private readonly ISender _sender;

        public AuthController(
            IAuthenticationManager authenticationManager,
            IGoogleAuthenticationService googleAuthenticationService,
            ISender sender)
        {
            _authenticationManager = authenticationManager;
            _googleAuthenticationService = googleAuthenticationService;
            _sender = sender;
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

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshToken)
        {
            var token = await _authenticationManager.GenerateNewTokenAsync(refreshToken.RefreshToken, HttpContext.RequestAborted);

            return Ok(token);
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest googleLoginRequest)
        {
            var token = await _googleAuthenticationService.HandleGoogleAuthenticationAsync(googleLoginRequest.AccessToken, HttpContext.RequestAborted);

            return Ok(token);
        }
    }
}
