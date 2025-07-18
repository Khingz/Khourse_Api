using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Khourse.Api.Models;

public class BaseModel
{
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}
