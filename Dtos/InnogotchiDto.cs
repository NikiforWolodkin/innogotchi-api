using innogotchi_api.Models;

namespace innogotchi_api.Dtos
{
    public class InnogotchiDto
    {
        public string Name { get; set; }
        public bool IsDead { get; set; }
        public int Age { get; set; }
        public int HappinessDayCount { get; set; }
        public string HungerLevel { get; set; }
        public string ThirstLevel { get; set; }
    }
}
