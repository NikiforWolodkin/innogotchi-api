namespace BusinessLayer.ResponseDtos
{
    public class FarmDto
    {
        public string Name { get; set; }
        public int AlivePetsCount { get; set; }
        public int DeadPetsCount { get; set; }
        public int AverageHappyDaysAmount { get; set; }
        public int AverageFeedingPeriod{ get; set; }
        public int AverageQuenchingPeriod { get; set; }
    }
}
