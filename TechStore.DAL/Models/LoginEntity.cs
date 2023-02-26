using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.DAL.Models;

public abstract class LoginEntity : BaseEntity
{
    [Required]
    [EmailAddress]
    [Column(TypeName = "TEXT")]
    public string Email { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    public string Password { get; set; }
}
