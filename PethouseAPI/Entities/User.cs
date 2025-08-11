namespace PethouseAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        
        public  string? Name { get; set; }

        public string? Address { get; set; }

        public  string? EmergencyContactName { get; set; }

        public  string? EmergencyContactPhone { get; set; }

        public  string? EmergencyContactRelationship { get; set; }

        public virtual ICollection<Pet>? Pets { get; set; }
    }
}
