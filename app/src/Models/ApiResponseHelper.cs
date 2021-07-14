using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bittn.Api.Services.Models;

namespace Bittn.Api.Models
{
    public static class ApiResponseHelper
    {
        public static ActionResult<ApiResponse> Ok(string message)
        {
            return Ok(message,  default(object));
        }

        public static ActionResult<ApiResponse> Ok<T>(string message, T data)
        {
            return ObjectResponse(StatusCodes.Status200OK, true, message: message, content: data);
        }

        public static ActionResult<ApiResponse> Ok<T>(string message, PagedResponse<T> data)
        {
            return ObjectResponse(StatusCodes.Status200OK, true, message: message, content: data);
        }

        public static ActionResult<ApiResponse> Fail(string message)
        {
            return Fail(message, default(object));
        }

        public static ActionResult<ApiResponse> Fail<T>(string message, T data)
        {
            return ObjectResponse(StatusCodes.Status200OK, false, message: message, content: data);
        }

        public static ActionResult<ApiResponse> BadRequest(string message)
        {
            return BadRequest(message, default(object));
        }

        public static ActionResult<ApiResponse> BadRequest<T>(string message, T data)
        {
            return ObjectResponse(StatusCodes.Status400BadRequest, false, message: message, content: data);
        }

        public static ActionResult<ApiResponse> Error(string message)
        {
            return Error(message, default(object));
        }

        public static ActionResult<ApiResponse> Error<T>(string message, T data)
        {
            return ObjectResponse(StatusCodes.Status500InternalServerError, false, message: message, content: data);
        }

        public static ActionResult<ApiResponse> ObjectResponse(int statusCode, bool success, string message)
        {
            var response = new ApiResponse
            {
                Success = success,
                Message = message
            };

            return new ObjectResult(response) { StatusCode = statusCode };
        }

        public static ActionResult<ApiResponse> ObjectResponse<T>(int statusCode, bool success, string message, T content)
        {
            var response = new ApiResponse
            {
                Success = success,
                Message = message
            };

            if (content != null)
            {
                response.Data = content;
            }

            return new ObjectResult(response) { StatusCode = statusCode };
        }

        public static ActionResult<ApiResponse> ObjectResponse<T>(int statusCode, bool success, string message, PagedResponse<T> content)
        {
            var response = new ApiResponse
            {
                Success = success,
                Message = message
            };

            if (content?.Data != null)
            {
                response.Data = content.Data;
                response.Navigation = new ApiNavigation
                {
                    PrevPageIndex = content.PrevPageIndex,
                    NextPageIndex = content.NextPageIndex
                };
            }

            return new ObjectResult(response) { StatusCode = statusCode };
        }
    }
}
