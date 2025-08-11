namespace PethouseAPI.Entities;

public class Appointment
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsTosAppointmentDocumentSigned { get; set; }
    public bool MedicalChecked { get; set; }
    public bool CarnetCheked { get; set; }
    public AppointmentType AppointmentType { get; set; }
    public virtual ICollection<PetAppointment>? PetsAppointments { get; set; }
}