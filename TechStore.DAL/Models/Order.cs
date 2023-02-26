using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.DAL.Models;

public class Order : BaseEntity
{
    [Required]
    [Column(TypeName = "DATETIME")]
    public DateTime OrderedAt { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? ReceivedAt { get; set; }

    public Review? Review { get; set; }
    public Client Client { get; set; }
    public IList<Product> Products { get; set; } = new List<Product>();
}
