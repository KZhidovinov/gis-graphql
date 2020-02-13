using GisApi.ApiServer.GraphTypes.Models;
using GisApi.DataAccessLayer;
using GisApi.DataAccessLayer.Models;
using GraphQL;
using GraphQL.Types;

namespace GisApi.ApiServer.GraphTypes
{
    public class MutationObject : ObjectGraphType
    {
        public MutationObject(IDbContext dbContext)
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
