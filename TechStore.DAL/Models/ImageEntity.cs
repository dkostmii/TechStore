using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.DAL.Models;

public class ImageEntity : BaseEntity
{
    [Required]
    [Column(TypeName = "TEXT")]
    [MinLength(3)]
    public required string FilePath { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [MinLength(7)]
    public required string ContentType { get; set; }
}
