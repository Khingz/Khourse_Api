namespace Khourse.Api.Dtos.ModuleDtos;

public class UpdateModuleRequestDto
{
    public string Title { get; set; } = string.Empty;
    public int? Position { get; set; }
    public int? EstimatedDurationMins { get; set; }

}
