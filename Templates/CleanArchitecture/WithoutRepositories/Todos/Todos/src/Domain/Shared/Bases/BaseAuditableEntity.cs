namespace Todos.Domain.Shared.Bases;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTimeOffset Created { get; set; }

    public string CreatedBy { get; set; } = "SYSTEM";

    public DateTimeOffset LastModified { get; set; }

    public string LastModifiedBy { get; set; } = "SYSTEM";
}
