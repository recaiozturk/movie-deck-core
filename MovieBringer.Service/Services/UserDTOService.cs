
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MovieBringer.Core.DTOs.MovieListDTO;
using MovieBringer.Core.DTOs;
using MovieBringer.Core.Entities;
using MovieBringer.Core.Repositories;
using MovieBringer.WebApp.Util.Abstract;
using Microsoft.EntityFrameworkCore;
using MovieBringer.Core.DTOs.UserDTO;
using Microsoft.AspNetCore.Http;
using MovieBringer.Core.Models.ViewModel.Profile;
using MovieBringer.Core.Services;

namespace MovieBringer.Service.Services
{
    public class UserDTOService:IUserDTOService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IMethods _methods;

        public UserDTOService(IMethods methods, IMapper mapper, UserManager<AppUser> userManager)
        {
            _methods = methods;
            _mapper = mapper;
            _userManager = userManager;
        }

        //get users
        public async Task<CustomResponseDto<IEnumerable<UserDTO>>> GetUserListAsync( )
        {
            var users = await _userManager.Users.ToListAsync();
            var usersMapped = _mapper.Map<IEnumerable<UserDTO>>(users);

            return CustomResponseDto<IEnumerable<UserDTO>>.Success(200, usersMapped);
        }

        //get user by id
        public async Task<CustomResponseDto<UserDTO>> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userMapped = _mapper.Map<UserDTO>(user);
                return CustomResponseDto<UserDTO>.Success(200, userMapped);
            }
            else
            {
                return CustomResponseDto<UserDTO>.Fail(404, "No User Found");
            }                      
        }

        //create user
        public async Task<CustomResponseDto<UserDTO>> CreateUserAsync(UserCreateDto userCreateModel)
        {
            var user = _mapper.Map<AppUser>(userCreateModel);
            var createdUserResult = await _userManager.CreateAsync(user, userCreateModel.Password);

            if (createdUserResult.Succeeded)
            {
                var userMapped= _mapper.Map<UserDTO>(user);
                return CustomResponseDto<UserDTO>.Success(200, userMapped);
            }
            else
            {
                return CustomResponseDto<UserDTO>.Fail(500, _methods.EnumurableToStringList(createdUserResult.Errors));

            }
        }

        //update user
        public async Task<CustomResponseDto<NoContentDto>> UpdateUserAsync(UserUpdateDto userUpdateModel)
        {
            var user = await _userManager.FindByIdAsync(userUpdateModel.Id);

            if (user != null)
            {
                //user= _mapper.Map<AppUser>(userUpdateModel); update de userId track sorunu verir
                user =_mapper.Map(userUpdateModel, user);

                var updatedUserResult = await _userManager.UpdateAsync(user);


                if (updatedUserResult.Succeeded)
                {
                    return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent,"User Updated");
                }
                else
                {
                    return CustomResponseDto<NoContentDto>.Fail(500, _methods.EnumurableToStringList(updatedUserResult.Errors));
                }
            }
            return CustomResponseDto<NoContentDto>.Fail(404, "No User Found");
        }

        //change password 
        public async Task<CustomResponseDto<UserDTO>> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                var changePassResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);


                if (changePassResult.Succeeded)
                {
                    return CustomResponseDto<UserDTO>.Success(StatusCodes.Status204NoContent, _mapper.Map<UserDTO>(user), "Succesfully Password Changed");
                }
                else
                {
                    return CustomResponseDto<UserDTO>.Fail(500, _methods.EnumurableToStringList(changePassResult.Errors));
                }
            }

            return CustomResponseDto<UserDTO>.Fail(404, "No User Found");
        }

        //delete
        public async Task<CustomResponseDto<NoContentDto>> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
            }
            else
            {
                return CustomResponseDto<NoContentDto>.Fail(404, "No User Found");
            }
        }
    }
}
