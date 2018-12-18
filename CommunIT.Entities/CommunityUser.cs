using System;

namespace CommunIT.Entities
{
    public class CommunityUser
    {
        public string UserId { get; set; }
        public int CommunityId { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsFavorite { get; set; }
        public virtual User User { get; set; }
        public virtual Community Community { get; set; }
    }
}