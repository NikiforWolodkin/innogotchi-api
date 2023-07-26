using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models
{
    public class Innogotchi
    {
        [Key]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public ICollection<FeedingAndQuenching> FeedingAndQuenchings { get; set; } = new List<FeedingAndQuenching>();
    }
}
