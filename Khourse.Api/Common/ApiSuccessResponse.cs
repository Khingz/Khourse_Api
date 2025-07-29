namespace Khourse.Api.Common;

public class ApiSuccessResponse<T>
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static ApiSuccessResponse<T> Ok(T data, string message = "Request successful")
    {
        return new ApiSuccessResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }
}
