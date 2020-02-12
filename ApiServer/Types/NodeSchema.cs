using GisApi.ApiServer.Types.Converters;
using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;

namespace GisApi.ApiServer.Types
{
    public class NodeSchema : Schema
    {
        public NodeSchema(NodeQuery query, NodeMutation mutation)
        {
            ValueConverter.Register(typeof(Dictionary<string, string>), typeof(Tags), ParseTags<string>);
            ValueConverter.Register(typeof(Dictionary<string, object>), typeof(Tags), ParseTags<object>);
            this.RegisterValueConverter(new TagsAstValueConverter());

            Query = query;
            Mutation = mutation;
        }

        private object ParseTags<T>(object tagsInput)
        {
            var res = new Tags();
            if (tagsInput is IDictionary<string, T> dict)
            {
                foreach (var pair in dict)
                {
                    res.Add(pair.Key, pair.Value.ToString());
                }
            }

            return res;
        }
    }
}
