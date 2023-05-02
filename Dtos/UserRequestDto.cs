namespace innogotchi_api.Dtos
{
    public class UserRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? OldPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
