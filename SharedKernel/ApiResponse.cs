using System.Net;

namespace SharedKernel
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public int? ErrorCode { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse<T> Sucess(T data, string? message = null)
        {

            Success = true;
            Data = data;
            Message = message;
            return this;
        }

        public ApiResponse<T> Failure(T? data, int? errorCode = null, string? message = null, Exception? exception = null, List<string>? errors = null)
        {
            if (exception is not null)
            {
                //message = exception.StackTrace;
                message = exception.Message;
            }
            Success = false;
            ErrorCode = errorCode;
            Message = message;
            Data = data;
            Errors = errors;
            return this;
        }

    }
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public ApiResponse<T> ApiReponse { get; set; } = new ApiResponse<T>();

        public Response<T> Sucess(T data, string? message = null, HttpStatusCode? statusCode = null)
        {
            message = message is not null ? message : "System success";
            StatusCode = statusCode is null ? HttpStatusCode.OK : statusCode.Value;
            ApiReponse = new ApiResponse<T>().Sucess(data, message);
            return this;
        }
        public Response<T> Failure(T? data, int? errorCode = null, string? message = null, Exception? exception = null, List<string>? errors = null, HttpStatusCode? statusCode = null)
        {
            StatusCode = statusCode is null ? HttpStatusCode.BadRequest : statusCode.Value;
            ApiReponse = new ApiResponse<T>().Failure(data, errorCode, message, exception, errors);
            return this;
        }
    }

}
