using GisApi.ApiServer.Types.Models;
using GisApi.DataAccessLayer;
using GisApi.DataAccessLayer.Models;
using GraphQL.Types;

namespace GisApi.ApiServer.Types
{
    public class NodeMutation : ObjectGraphType
    {
        public NodeMutation(IDbContext dbContext)
        {
            Field<NodeType>(
                "createNode",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<NodeInputType>> { Name = "item" }),
                resolve: context =>
                {
                    var node = context.GetArgument<Node>("item");
                    dbContext.Nodes.Add(node).Context.SaveChanges();

                    return node;
                });
        }
    }
}
