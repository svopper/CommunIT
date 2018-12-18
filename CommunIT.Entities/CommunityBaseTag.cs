using System;
using System.Collections.Generic;
using System.Text;

namespace CommunIT.Entities
{
    public class CommunityBaseTag
    {
        public int CommunityId { get; set; }
        public int BaseTagId { get; set; }

        public virtual Community Community { get; set; }
        public virtual BaseTag BaseTag { get; set; }
    }
}
