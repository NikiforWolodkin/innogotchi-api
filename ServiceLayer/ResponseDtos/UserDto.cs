namespace DataLayer.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? AvatarId { get; set; }
        public string? FarmName { get; set; }
    }
}
