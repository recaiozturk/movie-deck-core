using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBringer.Core.DTOs;

namespace MovieBringer.WebApp.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        //her defasında api de return yazmaya gerek kalmaması için,non action endpoint olmamasını saglar
        [NonAction]
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            if (response.StatusCode == 204)
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };


        }
    }
}
