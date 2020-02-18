namespace GisApi.ApiServer.GraphTypes.Models
{
    using System.Collections.Generic;
    using GisApi.ApiServer.GraphTypes.Scalars;
    using GisApi.DataAccessLayer.Models;
    using GisApi.DataAccessLayer.Repositories;
    using GraphQL.Types;

    public class NodeType : ObjectGraphType<Node>
    {
        public NodeType(IWayRepository wayRepository)
        {
            this.Name = "Node";
            this.Description = "Represents OpenStreetMap Node object";

            this.Field(x => x.Id, type: typeof(IdGraphType))
                .Description("The ID of the Node.");

            this.Field(x => x.OsmId, type: typeof(LongGraphType))
                .Description("The OSM ID of the Node.");

            this.Field(x => x.Tags, type: typeof(TagsType))
                .Description("Tags of the Node.");

            this.Field(x => x.Location, type: typeof(GeometryType))
                .Description("Location of the Node.");

            //Field(x => x.WayNodes, type: typeof(ListGraphType<WayNodeType>))
            //    .Description("List of WayNode objects");

            this.FieldAsync<ListGraphType<WayNodeType>, List<WayNode>>(
                nameof(Node.WayNodes), @"List of related WayNode objects",
                resolve: context => wayRepository.GetWayNodesAsync(context.Source, context.CancellationToken));
        }
    }
}