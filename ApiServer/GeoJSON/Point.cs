using Newtonsoft.Json;

namespace GisApi.ApiServer.GeoJSON
{
    public class Point
    {
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public string Type => "Point";

        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        public double[] Coordinates { get; set; }
    }
}
