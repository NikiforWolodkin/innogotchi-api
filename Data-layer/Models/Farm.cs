using System.ComponentModel.DataAnnotations;

namespace Data_layer.Models
{
    public class Farm
    {
        [Key]
        public string Name { get; set; }
        public ICollection<Innogotchi> Innogotchis { get; set; } = new List<Innogotchi>();
        public ICollection<Collaboration> Collaborations { get; set; } = new List<Collaboration>();
    }
}
