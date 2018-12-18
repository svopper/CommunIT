using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommunIT.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public string Content { get; set; }
        public string UserId { get; set; }
        public int ThreadId { get; set; }

        public virtual User User { get; set; }
        public virtual Thread Thread { get; set; }
    }
}
