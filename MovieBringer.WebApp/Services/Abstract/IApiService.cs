

using Microsoft.AspNetCore.Http.HttpResults;
using MovieBringer.Core.DTOs;

namespace MovieBringer.WebApp.Services.Abstract
{
    public interface IApiService
    {
        Task<CustomResponseDto<object>> GetAsync(string url);
        Task<CustomResponseDto<object>> PostAsync(string apiUrl, object model);

        Task<CustomResponseDto<NoContentDto>> PutAsync(string apiUrl, object model);
        Task<CustomResponseDto<NoContent>> DeleteAsync(string url);
    }
}
