namespace GisApi.ApiServer.GeoJSON
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Point
    {
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public string GeometryType => "Point";

        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        public List<double> Coordinates { get; set; }
    }
}
