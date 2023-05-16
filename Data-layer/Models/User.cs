using Microsoft.EntityFrameworkCore;

namespace DataLayer.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Avatar? Avatar { get; set; }
        public Farm? Farm { get; set; }
        public ICollection<Collaboration> Collaborations { get; set; } = new List<Collaboration>();
    }
}
