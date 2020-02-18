namespace GisApi.ApiServer.GraphTypes
{
    using System.Collections.Generic;
    using GisApi.ApiServer.GraphTypes.Models;
    using GisApi.DataAccessLayer;
    using GisApi.DataAccessLayer.Models;
    using GraphQL;
    using GraphQL.Types;

    public class MutationObject : ObjectGraphType
    {
        public MutationObject(IDbContext dbContext)
        {
            this.Field<ListGraphType<NodeType>>(
                "node",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ListGraphType<NonNullGraphType<NodeInputType>>>>
                    { Name = "items", Description = "List of new nodes" }),
                resolve: context =>
                {
                    var nodes = context.GetArgument<List<Node>>("items");
                    dbContext.Nodes.UpdateRange(nodes);
                    dbContext.SaveChanges();

                    return nodes;
                });

            this.Field<ListGraphType<WayType>>(
                "way",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ListGraphType<NonNullGraphType<WayInputType>>>>
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
