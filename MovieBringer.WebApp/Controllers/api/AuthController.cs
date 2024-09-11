
using Microsoft.AspNetCore.Mvc;
using MovieBringer.Core.DTOs;
using MovieBringer.Core.Models.ViewModel.Account;
using MovieBringer.Core.Services;
using MovieBringer.Service.Services;


namespace MovieBringer.WebApp.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthDtoService _authDtoService;

        public AuthController(IAuthDtoService authDtoService)
        {
            _authDtoService = authDtoService;
        }

        [HttpPost("register")]
        public async Task<CustomResponseDto<NoContentDto>> CreateUser([FromBody] RegisterViewModel model)
        {
            return await _authDtoService.CreateUserAsync(model);
        }

        [HttpPost("login")]
        public async Task<CustomResponseDto<object>> Login(LoginViewModel model)
        {
            return await _authDtoService.Login(model);
        }

        [HttpPost("generate-forget-password")]
        public async Task<CustomResponseDto<NoContentDto>> ForgotPassword(ForgetPasswordViewModel model)
        {
            return await _authDtoService.ForgotPassword(model);
        }

        [HttpPost("verify-reset-password")]
        public async Task<CustomResponseDto<NoContentDto>> ResetPassword(ResetPasswordViewModel model)
        {
            return await _authDtoService.ResetPassword(model);
        }

        [HttpGet("log-out")]
        public async Task<CustomResponseDto<NoContentDto>> LogOut()
        {
            return await _authDtoService.LogOut();
        }
    }
}
