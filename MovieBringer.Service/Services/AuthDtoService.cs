using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MovieBringer.Core.DTOs;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Models.ViewModel.Account;
using MovieBringer.Core.Services;
using MovieBringer.WebApp.Util;
using MovieBringer.WebApp.Util.Abstract;
using System.Web;

namespace MovieBringer.Service.Services
{
    public class AuthDtoService:IAuthDtoService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMyTokenHandler _tokenHandler;
        private readonly IMapper _mapper;
        private readonly IMethods _methods;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;
        private string _baseWebUrl;

        public AuthDtoService(IMethods methods, IMapper mapper, UserManager<AppUser> userManager, IEmailSender emailSender, IConfiguration config, SignInManager<AppUser> signInManager, IMyTokenHandler tokenHandler)
        {
            _methods = methods;
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
            _config = config;
            _baseWebUrl = _config.GetSection("AppSettings:WebUrl").Value;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }
        //create-register user
        public async Task<CustomResponseDto<NoContentDto>> CreateUserAsync(RegisterViewModel registerViewModel)
        {
            AppUser user = _mapper.Map<AppUser>(registerViewModel);

            var createdUserResult = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (createdUserResult.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = $"{_baseWebUrl}/Account/ConfirmEmail?userId={user.Id}&token={HttpUtility.UrlEncode(token)}";
                string mailBody = $"Please confirm your e-mail by clicking <a href='{url}'>here</a>.";
                string mailSubject = "Account Confirm";

                await _emailSender.SendSimpleAsync(user.Email, mailSubject, mailBody);

                return CustomResponseDto<NoContentDto>.Success(200,"User registered successfully. Please check your email for confirmation.");
            }
            else
            {
                return CustomResponseDto<NoContentDto>.Fail(500, createdUserResult.Errors.Select(e => e.Description).ToList());

            }
        }

        //login
        public async Task<CustomResponseDto<object>> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return CustomResponseDto<object>.Fail(404, "No User Found");
            }

            await _signInManager.SignOutAsync();

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return CustomResponseDto<object>.Fail(300, "Please approve email address");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var token_ = _tokenHandler.GenerateToken(user);
                return CustomResponseDto<object>.Success(200,token_, "Success Login");
            }
            else
            {
                return CustomResponseDto<object>.Fail(300, "Invalid email or password");
            }
            
        }

        //forgot password
        public async Task<CustomResponseDto<NoContentDto>> ForgotPassword(ForgetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return CustomResponseDto<NoContentDto>.Fail(300, "Invalid email or unconfirmed email");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = $"{_baseWebUrl}/Account/ResetPassword?Id={user.Id}&token={HttpUtility.UrlEncode(token)}";

            await _emailSender.SendSimpleAsync(model.Email, "Reset Password", $"For reset your password <a href='{url}'>click</a> ");
            return CustomResponseDto<NoContentDto>.Success(200, "Password reset instructions sent to your email.");
            
        }

        //verify reset password
        public async Task<CustomResponseDto<NoContentDto>> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if(user==null)
                return CustomResponseDto<NoContentDto>.Fail(404, "No User Found");

            model.Token = HttpUtility.UrlDecode(model.Token);
            model.Token = model.Token.Replace(" ", "+");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return CustomResponseDto<NoContentDto>.Success(200, "Email confirmed successfully.");
            }
            else
            {
                return CustomResponseDto<NoContentDto>.Fail(404, result.Errors.Select(e => e.Description).ToList());
            }
        }

        //logout
        public async Task<CustomResponseDto<NoContentDto>> LogOut()
        {
            await _signInManager.SignOutAsync();
            return CustomResponseDto<NoContentDto>.Success(200, "Successefuly Log Out");
        }

    }
}
