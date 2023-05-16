using DataLayer.Enums;

namespace DataLayer.Models
{
    public class Collaboration
    {
        public string FarmName { get; set; }
        public Guid UserId { get; set; }
        public Farm Farm { get; set; }
        public User User { get; set; }
        public CollaborationStatus Status { get; set; }
    }
}
