using System;
using System.Collections.Generic;

namespace CommunIT.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int CommunityId { get; set; }
        public virtual ICollection<EventUser> EventUsers { get; set; }
        public virtual Community Community { get; set; }
    }
}