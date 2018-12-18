using System;
using System.Collections.Generic;
using System.Text;

namespace CommunIT.Shared.Portable.DTOs.Event
{
    public class EventCreateDto
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int CommunityId { get; set; }
    }
}
