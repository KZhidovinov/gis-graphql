using GisApi.ApiServer.GraphTypes.Models;
using GisApi.DataAccessLayer;
using GisApi.DataAccessLayer.Models;
using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;

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

            Field<ListGraphType<WayType>>(
                "way",
                arguments: new QueryArguments(
                    new QueryArgument<
                        NonNullGraphType<ListGraphType<NonNullGraphType<WayInputType>>>>
                    { Name = "items", Description = "List of new ways" }
                ),
                resolve: context =>
                {
                    var ways = context.GetArgument<List<Way>>("items");
                    dbContext.Ways.UpdateRange(ways);
                    dbContext.SaveChanges();

                    return ways;
                });
        }
    }
}
