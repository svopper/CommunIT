using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunIT.Entities
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public string University { get; set; }
        public DateTime Created { get; set; }
        
        public virtual ICollection<CommunityUser> CommunityUsers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}