using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.DAL.Models;

public abstract class UserEntity : LoginEntity
{
    [Required]
    [StringLength(maximumLength: 60, MinimumLength = 2)]
    [MinLength(2)]
    [MaxLength(60)]
    [Column(TypeName = "TEXT")]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(120)]
    [Column(TypeName = "TEXT")]
    public string LastName { get; set; }
}
