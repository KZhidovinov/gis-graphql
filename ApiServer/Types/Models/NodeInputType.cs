using GraphQL.Types;

namespace GisApi.ApiServer.Types.Models
{
    public class NodeInputType : InputObjectGraphType
    {
        public NodeInputType()
        {
            Name = "NodeInput";
            Field<NonNullGraphType<LongGraphType>>("osmId");
        }
    }
}