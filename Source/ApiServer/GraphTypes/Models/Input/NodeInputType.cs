namespace GisApi.ApiServer.GraphTypes.Models
{
    using GisApi.ApiServer.GraphTypes.Scalars;
    using GraphQL.Types;

    public class NodeInputType : InputObjectGraphType
    {
        public NodeInputType()
        {
            this.Name = "NodeInput";
            this.Description = "Input object to enter Node";

            this.Field<IdGraphType>("id", "ID of the Node");

            this.Field<LongGraphType>("osmId", "OpenStreetMap ID if available");

            this.Field<TagsType>("tags", "Node tags as JSON object");

            this.Field<GeometryType>("location", "Location of Node as GeoJSON Point");
        }
    }
}