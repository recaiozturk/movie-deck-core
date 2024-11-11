using System.Net;
using System.Text.Json.Serialization;

namespace MovieDeck.Service.Shared
{
    public class ApiServiceResult
    {
        public List<string>? Errors { get; set; }


        [JsonIgnore] public bool IsSuccess => Errors is null || Errors.Count == 0;
        [JsonIgnore] public bool IsFail => !IsSuccess;

        [JsonIgnore] public HttpStatusCode Status { get; set; }

        public static ApiServiceResult Success(HttpStatusCode status)
        {
            return new ApiServiceResult() { Status = status };
        }

        public static ApiServiceResult Fail(List<string> errors, HttpStatusCode status)
        {
            return new ApiServiceResult
            {
                Errors = errors,
                Status = status
            };
        }

        public static ApiServiceResult Fail(string error, HttpStatusCode status)
        {
            return new ApiServiceResult
            {
                Errors = [error],
                Status = status
            };
        }
    }


    public class ApiServiceResult<T> : ApiServiceResult
    {
        public T? Data { get; set; }


        public static ApiServiceResult<T> Success(T data, HttpStatusCode status)
        {
            return new ApiServiceResult<T>
            {
                Data = data,
                Status = status
            };
        }


        public new static ApiServiceResult<T> Fail(List<string> errors, HttpStatusCode status)
        {
            return new ApiServiceResult<T>
            {
                Errors = errors,
                Status = status
            };
        }

        public new static ApiServiceResult<T> Fail(string error, HttpStatusCode status)
        {
            return new ApiServiceResult<T>
            {
                Errors = [error],
                Status = status
            };
        }
    }
}
