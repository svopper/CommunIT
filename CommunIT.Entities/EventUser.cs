namespace CommunIT.Entities
{
    public class EventUser
    {
        public int EventId { get; set; }
        public string UserId { get; set; }
        
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
}