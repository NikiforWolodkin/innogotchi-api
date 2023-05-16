using BusinessLayer.Enums;

namespace BusinessLayer.ResponseDtos
{
    public class InnogotchiDto
    {
        public string Name { get; set; }
        public bool IsDead { get; set; }
        public int Age { get; set; }
        public InnogotchiHungerLevel HungerLevel { get; set; }
        public InnogotchiThirstLevel ThirstLevel { get; set; }
        public int HappyDays { get; set; }
    }
}
