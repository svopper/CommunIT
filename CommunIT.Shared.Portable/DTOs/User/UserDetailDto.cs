using System;

namespace CommunIT.Shared.Portable.DTOs.User
{
    public class UserDetailDto
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string University { get; set; }
        public string Bio { get; set; }
        public DateTime Created { get; set; }
    }
}