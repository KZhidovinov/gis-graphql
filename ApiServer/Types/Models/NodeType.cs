using GisApi.DataAccessLayer.Models;
using GraphQL.Types;

namespace GisApi.ApiServer.Types.Models
{
    public class NodeType : ObjectGraphType<Node>
    {
        public NodeType()
        {
            Name = "Node";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Node.");
            Field(x => x.OsmId, type: typeof(LongGraphType)).Description("The OSM ID of the Node.");
            Field(x => x.Tags, type: typeof(TagsType)).Description("Tags of the Node.");
            //Field<Point>(x => x.Location).Description("Location of the Node.");
        }
    }
}