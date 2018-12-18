using System;
using System.ComponentModel.DataAnnotations;

namespace CommunIT.Entities
{
    public class Forum
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public int CommunityId { get; set; }

        public virtual Community Community { get; set; }
    }
}