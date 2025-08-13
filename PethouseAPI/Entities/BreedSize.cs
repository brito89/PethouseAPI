using System.ComponentModel.DataAnnotations;

namespace PethouseAPI.Entities;

public class BreedSize
{
    public int Id { get; set; }
    [MaxLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
    public required string Name { get; set; }
    [MaxLength(200, ErrorMessage = "Label cannot be longer than 200 characters.")]
    public string? Label { get; set; }
    public decimal PricePeakSeason { get; set; }
    public decimal PriceLowSeason { get; set; }
    //public virtual ICollection<Pet>? Pets { get; set; }
}