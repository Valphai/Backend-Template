using System.ComponentModel.DataAnnotations;

namespace Project.Domain.Entities;

public abstract class EntityBase
{
    [Key]
    public Guid Id { get; } = Guid.NewGuid();
        
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateUpdated { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
}