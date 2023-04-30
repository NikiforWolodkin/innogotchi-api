namespace innogotchi_api.Models
{
    public class Collaboration
    {
        public string FarmName { get; set; }
        public string UserEmail { get; set; }
        public Farm Farm { get; set; }
        public User User { get; set; }
    }
}
