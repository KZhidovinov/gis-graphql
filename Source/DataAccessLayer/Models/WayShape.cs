using NetTopologySuite.Geometries;

namespace GisApi.DataAccessLayer.Models
{
    public class WayShape
    {
        public long WayId { get; set; }
        public LineString Shape { get; set; }
        public virtual Way Way { get; set; }
    }
}
