using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;

namespace GisApi.ApiServer.Types
{
    public class NodeSchema : Schema
    {
        public NodeSchema(NodeQuery query, NodeMutation mutation)
        {
            ValueConverter.Register(typeof(Dictionary<string, object>), typeof(Tags), ParseTags);

            Query = query;
            Mutation = mutation;
        }

        private object ParseTags(object tagsInput)
        {
            return tagsInput as Tags;
        }
    }
}
