using GisApi.ApiServer.GraphTypes.Scalars;
using GisApi.DataAccessLayer.Models;
using GraphQL.Types;

namespace GisApi.ApiServer.GraphTypes.Models
{
    public class WayNodeType : ObjectGraphType<WayNode>
    {
        public WayNodeType()
        {
            Name = "WayNodes";

            Field(x => x.WayIdx).Description("An integer Index of the Node in the Way");
            Field(x => x.Role).Description("The string Role of the Node.");

            Field(x => x.Way, type: typeof(WayType)).Description("Reference to Way");
            Field(x => x.Node, type: typeof(NodeType)).Description("Reference to Node");
        }
    }
}