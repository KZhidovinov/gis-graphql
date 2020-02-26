namespace GisApi.DataAccessLayer.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using NetTopologySuite.Features;

    public class Way
    {
        public long Id { get; set; }
        public long? OsmId { get; set; }
        public TagsDictionary Tags { get; set; }

        public virtual List<WayNode> WayNodes { get; set; }
        public virtual WayShape WayShape { get; set; }

        public virtual Feature Feature => this.WayShape == null
            ? null
            : new Feature(this.WayShape.Shape,
                new AttributesTable(this.Tags.Select(p => KeyValuePair.Create(p.Key, p.Value as object))));
    }
}