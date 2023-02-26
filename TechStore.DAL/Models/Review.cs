using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.DAL.Models;

public class Review : BaseEntity
{
    [Required]
    [Range(1, 5)]
    [Column(TypeName = "INTEGER")]
    public int Rate { get; set; }

    [Column(TypeName = "TEXT")]
    [MinLength(3)]
    [MaxLength(200)]
    public string? Caption { get; set; }
}
