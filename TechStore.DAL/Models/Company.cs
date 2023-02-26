using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.DAL.Models;

public class Company : BaseEntity
{
    [Required]
    [MinLength(3)]
    [MaxLength(60)]
    [Column(TypeName = "TEXT")]
    public string Name { get; set; }
}
