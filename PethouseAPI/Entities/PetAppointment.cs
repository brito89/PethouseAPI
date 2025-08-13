namespace PethouseAPI.Entities;

public class PetAppointment
{
    public int Id { get; set; }
    public int PetId { get; set; }
    public Pet? Pet { get; set; }
    public int AppointmentId { get; set; }    
    public Appointment? Appointment { get; set; }
    public bool Monday { get; set; } 
    public bool Tuesday { get; set; } 
    public bool Wednesday { get; set; } 
    public bool Thursday { get; set; } 
    public bool Friday { get; set; } 
    public bool IsActive { get; set; }
}