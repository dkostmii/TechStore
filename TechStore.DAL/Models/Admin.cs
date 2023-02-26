using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.DAL.Models;

public class Admin : UserEntity
{
    [Required]
    [MinLength(3)]
    [MaxLength(256)]
    [Column(TypeName = "TEXT")]
    public string UserName { get; set; }
}
