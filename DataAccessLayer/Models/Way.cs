using System.Collections.Generic;

namespace GisApi.DataAccessLayer.Models
{
    public class Way
    {
        public long Id { get; set; }
        public long OsmId { get; set; }
        public Dictionary<string, string> Tags { get; set; }

        public virtual List<WayNode> WayNodes { get; set; }
    }
}