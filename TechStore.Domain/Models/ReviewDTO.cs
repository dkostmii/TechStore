namespace TechStore.Domain.Models;

public class ReviewDTO : BaseEntityDTO
{
    public int Rate { get; set; }
    public string? Caption { get; set; }
}
