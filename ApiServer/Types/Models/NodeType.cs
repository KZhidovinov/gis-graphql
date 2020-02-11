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
            Field(x => x.OsmId).Description("The OSM ID of the Node.");
            //Field<ListGraphType>
            //Field<Dictionary<string, string>>(x => x.Tags).Description("Tags of the Node.");
            //Field<Point>(x => x.Location).Description("Location of the Node.");
        }
    }
}