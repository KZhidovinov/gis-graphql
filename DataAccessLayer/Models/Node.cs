namespace GisApi.DataAccessLayer.Models
{
    using System.Collections.Generic;
    using NetTopologySuite.Geometries;

    public class Node
    {
        public long Id { get; set; }
        public long? OsmId { get; set; }
        public Dictionary<string, string> Tags { get; set; }
        public Point Location { get; set; }

        public virtual List<WayNode> WayNodes { get; set; }
    }
}