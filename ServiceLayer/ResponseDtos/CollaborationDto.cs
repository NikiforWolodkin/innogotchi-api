namespace BusinessLayer.ResponseDtos
{
    public class CollaborationDto
    {
        public string FarmName { get; set; }
        public Guid CollaboratorId { get; set; }
        public string CollaboratorFirstName { get; set; }
        public string CollaboratorLastName { get; set; }
    }
}
