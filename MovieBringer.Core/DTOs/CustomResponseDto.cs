using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieBringer.Core.DTOs
{
    public class CustomResponseDto<T>
    {
        public T Data { get; set; }


        //status code cliente oto gidiyor sade biz kullanammız için cliente giderken ignore ettik
        [JsonIgnore]
        public int StatusCode { get; set; }


        public List<String> Errors { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }


        //static factory method
        public static CustomResponseDto<T> Success(int statusCode, T data, string successMessage)
        {
            return new CustomResponseDto<T> { IsSuccess = true, StatusCode = statusCode,Message=successMessage,Data=data };
        }

        public static CustomResponseDto<T> Success(int statusCode, string successMessage)
        {
            return new CustomResponseDto<T> {IsSuccess=true, StatusCode = statusCode, Message = successMessage };
        }

        public static CustomResponseDto<T> Success(int statusCode, T data)
        {
            return new CustomResponseDto<T> { IsSuccess = true, Data = data, StatusCode = statusCode };
        }
        public static CustomResponseDto<T> Success(int statusCode)
        {
            return new CustomResponseDto<T> { IsSuccess = true, StatusCode = statusCode };
        }

        public static CustomResponseDto<T> Fail(int statusCode, List<string> errors)
        {
            return new CustomResponseDto<T> { IsSuccess = false, StatusCode = statusCode, Errors = errors };
        }

        public static CustomResponseDto<T> Fail(int statusCode, string error)
        {
            return new CustomResponseDto<T> { IsSuccess = false, StatusCode = statusCode, Errors = new List<string> { error } };
        }
    }
}
