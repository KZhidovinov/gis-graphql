using GraphQL.Types;

namespace GisApi.ApiServer.Types
{
    public class NodeSchema : Schema
    {
        public NodeSchema(NodeQuery query, NodeMutation mutation)
        {
            Query = query;
            Mutation = mutation;
        }
    }
}
