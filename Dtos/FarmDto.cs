namespace innogotchi_api.Dtos
{
    public class FarmDto
    {
        public string Name { get; set; }
        public int AverageFeedingPeriod { get; set; }
        public int AverageQuenchingPeriod { get; set; }
        public int AverageHappinessDayCount { get; set; }
        public int AlivePetsCount { get; set; }
        public int DeadPetsCount { get; set; }
        public int AveragePetAge { get; set; }
    }
}
