using MovieBringer.Core.DTOs;
using MovieBringer.Core.DTOs.UserDTO;
using MovieBringer.Core.Models.ViewModel.Profile;

namespace MovieBringer.Core.Services
{
    public interface IUserDTOService
    {
        Task<CustomResponseDto<IEnumerable<UserDTO>>> GetUserListAsync();
        Task<CustomResponseDto<UserDTO>> GetUserByIdAsync(string id);
        Task<CustomResponseDto<UserDTO>> CreateUserAsync(UserCreateDto userCreateModel);
        Task<CustomResponseDto<NoContentDto>> UpdateUserAsync(UserUpdateDto userUpdateModel);
        Task<CustomResponseDto<UserDTO>> ChangePasswordAsync(ChangePasswordViewModel model);
        Task<CustomResponseDto<NoContentDto>> DeleteUserAsync(string id);
    }
}
