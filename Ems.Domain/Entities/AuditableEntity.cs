using System.ComponentModel.DataAnnotations;
using Ems.Core.Interfaces;

namespace Ems.Core.Entities;

public class AuditableEntity : IAuditableEntity
{
    [MaxLength(256)] public string CreatedBy { get; set; }

    [MaxLength(256)] public string UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }
    public DateTime CreatedDate { get; set; }
}