using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.DAL.Models;

public class Product : BaseEntity
{
    [Required]
    [MinLength(10)]
    [MaxLength(60)]
    [Column(TypeName = "TEXT")]
    public string Name { get; set; }

    [MinLength(10)]
    [MaxLength(1000)]
    [Column(TypeName = "TEXT")]
    public string Description { get; set; }

    [MinLength(1)]
    [MaxLength(20)]
    [Column(TypeName = "TEXT")]
    public string Model { get; set; }

    [Required]
    [Range(0.1, 1000000)]
    [Column(TypeName = "MONEY")]
    public decimal Price { get; set; }

    [Range(0, 1000000)]
    [Column(TypeName = "INTEGER")]
    public int Available { get; set; }

    [MinLength(3)]
    [MaxLength(60)]
    [Column(TypeName = "TEXT")]
    public string Country { get; set; }

    public IList<ImageEntity> Images { get; set; } = new List<ImageEntity>();

    [Required]
    public Company Supplier { get; set; }

    public IList<Order> Orders { get; set; } = new List<Order>();
}
