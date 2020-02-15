namespace GisApi.ApiServer.GraphTypes.Models
{
    using GisApi.ApiServer.GraphTypes.Scalars;
    using GraphQL.Types;

    public class NodeInputType : InputObjectGraphType
    {
        public NodeInputType()
        {
            Name = "NodeInput";
            Description = "Input object to enter Node";

            Field<LongGraphType>("osmId", "OpenStreetMap ID if available");

            Field<TagsType>("tags", "Node tags as JSON object");

            Field<PointType>("location", "Location of Node as GeoJSON Point");
        }
    }
}