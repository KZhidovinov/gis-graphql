namespace GisApi.ApiServer.GraphTypes.Models
{
    using GisApi.ApiServer.GraphTypes.Scalars;
    using GisApi.DataAccessLayer.Models;
    using GraphQL.Types;

    public class NodeType : ObjectGraphType<Node>
    {
        public NodeType()
        {
            Name = "Node";

            Field(x => x.Id, type: typeof(IdGraphType))
                .Description("The ID of the Node.");

            Field(x => x.OsmId, type: typeof(LongGraphType))
                .Description("The OSM ID of the Node.");

            Field(x => x.Tags, type: typeof(TagsType))
                .Description("Tags of the Node.");

            Field(x => x.Location, type: typeof(GeometryType))
                .Description("Location of the Node.");

            Field(x => x.WayNodes, type: typeof(ListGraphType<WayNodeType>))
                .Description("List of WayNode objects");
        }
    }
}