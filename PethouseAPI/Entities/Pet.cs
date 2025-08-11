using System.ComponentModel.DataAnnotations;

namespace PethouseAPI.Entities;

public class Pet
{
    public int Id { get; set; }
    [MaxLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
    public required string Name { get; set; }
    public DateOnly DateOfBirth { get; set; }
    [MaxLength(100, ErrorMessage = "BreedName cannot be longer than 100 characters.")]
    public string? BreedName { get; set; }
    public bool IsMedicated { get; set; }
    [MaxLength(500, ErrorMessage = "Notes cannot be longer than 500 characters.")]
    public string? Notes { get; set; }

    public int BreedSizeId { get; set; }
    public BreedSize? BreedSize { get; set; }

    public User? User { get; set; }
    
    public virtual ICollection<PetAppointment>? PetsAppointments { get; set; }
}