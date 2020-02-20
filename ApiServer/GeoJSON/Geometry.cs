namespace GisApi.ApiServer.GeoJSON
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Geometry
    {
        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public string GeometryType { get; set; }

        [JsonProperty(PropertyName = "coordinates", Required = Required.Always)]
        public List<object> Coordinates { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            if (obj is Geometry other)
            {
                return this.GeometryType == other.GeometryType
                    && this.Coordinates.Equals(other.Coordinates);
            }

            return false;
        }
    }
}