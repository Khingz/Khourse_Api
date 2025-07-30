using System;

namespace Khourse.Api.Dtos;

public class EmailDto
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string TemplateName { get; set; } = string.Empty;
    public object? Model { get; set; }
}
