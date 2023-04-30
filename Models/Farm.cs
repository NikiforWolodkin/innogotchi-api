using System.ComponentModel.DataAnnotations;

namespace innogotchi_api.Models
{
    public class Farm
    {
        [Key]
        public string Name { get; set; }
        public ICollection<Innogotchi> Innogotchis { get; set; }
        public ICollection<Collaboration> Collaborations { get; set; }
    }
}
