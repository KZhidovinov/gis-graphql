namespace GisApi.ApiServer.GraphTypes
{
    using System.Collections.Generic;
    using GisApi.ApiServer.GraphTypes.Models;
    using GisApi.DataAccessLayer.Models;
    using GisApi.DataAccessLayer.Repositories;
    using GraphQL;
    using GraphQL.Types;

    public class QueryObject : ObjectGraphType
    {
        public QueryObject(IWayRepository wayRepository)
        {
            this.FieldAsync<NodeType, Node>(
                name: "node",
                description: "Select first node",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id", Description = "The ID of the Node." }),
                resolve: context => wayRepository.GetNodeByIdAsync(context.GetArgument<long>("id")));

            this.FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<NodeType>>>, List<Node>>(
                name: "nodes",
                description: "All nodes",
                resolve: context => wayRepository.GetNodesAsync(context.CancellationToken));

            this.FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<WayType>>>, List<Way>>(
                name: "ways",
                description: "All ways",
                resolve: context =>
                    wayRepository.GetWaysAsync(context.CancellationToken,
                        includeFeature: context.SubFields.ContainsKey("feature"),
                        includeWayNodes: context.SubFields.ContainsKey("wayNodes")
                    )
            );
        }
    }
}