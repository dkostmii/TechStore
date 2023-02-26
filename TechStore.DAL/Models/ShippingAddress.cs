using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.DAL.Models;

public class ShippingAddress : BaseEntity
{
    [Column(TypeName = "TEXT")]
    [MinLength(10)]
    [MaxLength(200)]
    public string Address { get; set; }
}
