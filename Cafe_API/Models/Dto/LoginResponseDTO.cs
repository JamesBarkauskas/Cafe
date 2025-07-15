namespace Cafe_API.Models.Dto
{
    public class LoginResponseDTO
    {
        public LocalUser User { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
