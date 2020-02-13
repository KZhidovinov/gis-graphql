using GisApi.ApiServer.GraphTypes.Scalars;
using GraphQL.Types;

namespace GisApi.ApiServer.GraphTypes.Models
{
    public class WayNodeInputType : InputObjectGraphType
    {
        public WayNodeInputType()
        {
            Name = "WayNodesInput";
            Field<IntGraphType>("wayIdx");
            Field<StringGraphType>("role");

            Field<WayInputType>("way");
            Field<NodeInputType>("node");
        }
    }
}