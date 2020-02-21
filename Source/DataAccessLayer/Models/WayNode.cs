namespace GisApi.DataAccessLayer.Models
{
    public class WayNode
    {
        public long WayId { get; set; }
        public long NodeId { get; set; }
        public int WayIdx { get; set; }
        public string Role { get; set; }

        public virtual Way Way { get; set; }
        public virtual Node Node { get; set; }
    }
}