namespace Khourse.Api.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public ApiError? Error { get; set; }

    public static ApiResponse<T> Ok(string? message, T? data)
    {
        return new ApiResponse<T> { Success = true, Message = message, Data = data };
    }

    public static ApiResponse<T> Fail(int code, string type, string message)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Error = new ApiError
            {
                Code = code,
                Type = type,
                
            }
        };
    }
}

public class ApiError
{
    public int Code { get; set; }
    public string? Type { get; set; }
}
