using System.Collections.Generic;

namespace eShop.Shared.DTOs.Users
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public string JoinedDate { get; set; }
    }
}