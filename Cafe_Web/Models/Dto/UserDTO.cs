namespace Cafe_Web.Models.Dto
{
    public class UserDTO
    {
        // change id to string, bc Identity uses Guid of type str..
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
