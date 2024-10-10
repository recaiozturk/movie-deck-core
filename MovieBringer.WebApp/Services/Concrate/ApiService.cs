
using Microsoft.AspNetCore.Http.HttpResults;
using MovieBringer.Core.DTOs;
using MovieBringer.WebApp.Services.Abstract;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MovieBringer.WebApp.Services.Concrate
{
    public class ApiService : IApiService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public ApiService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public async Task<CustomResponseDto<object>> GetAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"];
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var call = await httpClient.GetAsync(url);
                var apiResponse = await call.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<CustomResponseDto<object>>(apiResponse);
                return result;
            }
        }

        public async Task<CustomResponseDto<NoContent>> DeleteAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"];
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var call = await httpClient.DeleteAsync(url);
                var apiResponse = await call.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<CustomResponseDto<NoContent>>(apiResponse);
                return result;
            }
        }

        public async Task<CustomResponseDto<object>> PostAsync(string apiUrl, object model)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PostAsJsonAsync(apiUrl, model);

            if (!response.IsSuccessStatusCode) return null;

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<object>>();

            return responseBody;
        }

        public async Task<CustomResponseDto<NoContentDto>> PutAsync(string apiUrl, object model)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["AccessToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.PutAsJsonAsync(apiUrl, model);
            if (!response.IsSuccessStatusCode) return null;
            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<NoContentDto>>();
            return responseBody;

        }
    }
}
