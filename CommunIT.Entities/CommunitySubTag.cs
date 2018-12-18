using System;
using System.Collections.Generic;
using System.Text;

namespace CommunIT.Entities
{
    public class CommunitySubTag
    {
        public int CommunityId { get; set; }
        public int SubTagId { get; set; }

        public virtual Community Community { get; set; }
        public virtual SubTag SubTag { get; set; }
    }
}
