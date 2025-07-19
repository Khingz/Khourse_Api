namespace Khourse.Api.Common;

public class ApiResponse<T>
{
    public string Status { get; set; } = "success";
    public string Message { get; set; }
    public T? Data { get; set; }
    public object? Errors { get; set; }

    // Success with data
    public ApiResponse(T data, string message = "Request successful.")
    {
        Status = "success";
        Message = message;
        Data = data;
    }

    // Success without data
    public ApiResponse(string message = "Request successful.")
    {
        Status = "success";
        Message = message;
    }

    // Failure
    public ApiResponse(string message, object? errors)
    {
        Status = "error";
        Message = message;
        Errors = errors;
    }

    // Static helpers 
    public static ApiResponse<T> Success(T data, string message = "Request successful.") =>
        new(data, message);

    public static ApiResponse<T> Success(string message = "Request successful.") =>
        new(message);

    public static ApiResponse<T> Fail(string message, object? errors = null) =>
        new(message, errors) { Status = "error" };
}
