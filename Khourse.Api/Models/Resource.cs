using System;
using System.ComponentModel.DataAnnotations.Schema;
using Khourse.Api.Enums;

namespace Khourse.Api.Models;

[Table("resources")]
public class Resource : BaseModel
{
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("type")]
    public ResourceTypeEnum Type { get; set; }

    [Column("resource_url")]
    public string ResourceUrl { get; set; } = string.Empty;

    [Column("module_id")]
    public Guid? ModuleId { get; set; }

    [Column("module")]
    public Module? Module { get; set; }
}
