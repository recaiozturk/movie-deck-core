using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieBringer.Core.DTOs;
using MovieBringer.Core.DTOs.UserDTO;
using MovieBringer.Core.Models.ViewModel.Profile;
using MovieBringer.Core.Services;
using MovieBringer.Service.Services;

namespace MovieBringer.WebApp.Controllers.api
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserDTOService _userDtoService;

        public UserController(UserDTOService userDtoService)
        {
            _userDtoService = userDtoService;
        }

        [HttpGet]
        public async Task<CustomResponseDto<IEnumerable<UserDTO>>> Get()
        {
            return await _userDtoService.GetUserListAsync();
        }

        [HttpGet("{id}")]
        public async Task<CustomResponseDto<UserDTO>> Get(string id)
        {
            return await _userDtoService.GetUserByIdAsync(id);
        }

        [HttpPost]
        public async Task<CustomResponseDto<UserDTO>> Post(UserCreateDto userCreateDto)
        {
            return await _userDtoService.CreateUserAsync(userCreateDto);
        }


        [HttpPut]
        public async Task<CustomResponseDto<NoContentDto>> Put(UserUpdateDto userUpdateDto)
        {
            return await _userDtoService.UpdateUserAsync(userUpdateDto);
        }

        [HttpPost("change-password")]
        public async Task<CustomResponseDto<UserDTO>> ChangePassword(ChangePasswordViewModel changePasswordViewModelc)
        {
            return await _userDtoService.ChangePasswordAsync(changePasswordViewModelc);
        }

        [HttpDelete("{id}")]
        public async Task<CustomResponseDto<NoContentDto>> Delete(string id)
        {
            return await _userDtoService.DeleteUserAsync(id);
        }

    }
}
