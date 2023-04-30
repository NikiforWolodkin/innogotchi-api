using System.ComponentModel.DataAnnotations;

namespace innogotchi_api.Models
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Farm> Farms { get; set; }
        public ICollection<Collaboration> Collaborations { get; set; }
    }
}
