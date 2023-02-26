using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechStore.DAL.Models;

public abstract class BaseEntity
{
    [Key]
    [Column(TypeName = "INTEGER")]
    public int Id { get; set; }
}
