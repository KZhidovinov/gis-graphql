using System.Collections.Generic;
using Newtonsoft.Json;

namespace GisApi.ApiServer.GeoJSON
{
    public class Feature
    {
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public string ObjectType { get; set; }

        [JsonProperty(PropertyName = "properties", Required = Required.Always)]
        public Dictionary<string, object> Properties { get; set; }

        [JsonProperty(PropertyName = "bbox", NullValueHandling = NullValueHandling.Ignore)]
        public List<double> BoundingBox { get; set; }

        [JsonProperty(PropertyName = "geometry", Required = Required.Always)]
        public Geometry Geometry { get; set; }
    }
}
