using System;

namespace Khourse.Api.Common;

public class ApiErrorResponse
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = "An error occurred";
    public int StatusCode { get; set; }
    public IEnumerable<ErrorDetail> Errors { get; set; } = Enumerable.Empty<ErrorDetail>();

    public static ApiErrorResponse Fail(int statusCode, string message, IEnumerable<ErrorDetail>? errors = null)
    {
        return new ApiErrorResponse
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Errors = errors ?? new List<ErrorDetail>()
        };
    }
}

public class ErrorDetail
{
    public string Code { get; set; } = default!;
    public string Description { get; set; } = default!;
}