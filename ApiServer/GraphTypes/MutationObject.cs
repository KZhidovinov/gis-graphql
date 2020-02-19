namespace GisApi.ApiServer.GraphTypes
{
    using System.Collections.Generic;
    using GisApi.ApiServer.GraphTypes.Models;
    using GisApi.DataAccessLayer.Models;
    using GisApi.DataAccessLayer.Repositories;
    using GraphQL;
    using GraphQL.Types;

    public class MutationObject : ObjectGraphType
    {
        public MutationObject(IWayRepository wayRepository)
        {
            this.FieldAsync<ListGraphType<NodeType>, List<Node>>(
                "node",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ListGraphType<NonNullGraphType<NodeInputType>>>> { Name = "items", Description = "List of new nodes" }),
                resolve: async context =>
                {
                    var nodes = context.GetArgument<List<Node>>("items");
                    await wayRepository.CreateOrUpdateNodesAsync(nodes, context.CancellationToken)
                        .ConfigureAwait(false);
                    return nodes;
                });

            this.FieldAsync<ListGraphType<WayType>, List<Way>>(
                "way",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ListGraphType<NonNullGraphType<WayInputType>>>> { Name = "items", Description = "List of new ways" }
                ),
                resolve: async context =>
                {
                    var ways = context.GetArgument<List<Way>>("items");

                    await wayRepository.CreateOrUpdateWaysAsync(ways, context.CancellationToken)
                        .ConfigureAwait(false);

                    return ways;
                });
        }
    }
}