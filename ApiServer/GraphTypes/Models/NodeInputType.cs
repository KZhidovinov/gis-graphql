using GisApi.ApiServer.GraphTypes.Scalars;
using GraphQL.Types;

namespace GisApi.ApiServer.GraphTypes.Models
{
    public class NodeInputType : InputObjectGraphType
    {
        public NodeInputType()
        {
            Name = "NodeInput";
            Field<NonNullGraphType<LongGraphType>>("osmId");
            Field<TagsType>("tags");
            Field<PointType>("location");
        }
    }
}