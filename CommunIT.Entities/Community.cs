using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunIT.Entities
{
    public class Community
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Forum> Forums { get; set; }
        public virtual ICollection<CommunityUser> CommunityUsers { get; set; }
        public virtual ICollection<CommunityBaseTag> CommunityBaseTags { get; set; }
        public virtual ICollection<CommunitySubTag> CommunitySubTags { get; set; }

    }
}