using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunIT.Entities
{
    public class Thread
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public int ForumId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Forum Forum { get; set; }
    }
}