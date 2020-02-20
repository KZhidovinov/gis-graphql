namespace GisApi.ApiServer.GraphTypes.Models
{
    using System.Collections.Generic;
    using GisApi.ApiServer.GraphTypes.Scalars;
    using GisApi.DataAccessLayer.Models;
    using GraphQL.Types;

    public class NodeType : ObjectGraphType<Node>
    {
        public NodeType()
        {
            this.Name = "Node";
            this.Description = "Represents OpenStreetMap Node object";

            this.Field(x => x.Id, type: typeof(LongGraphType))
                .Description("The ID of the Node.");

            this.Field(x => x.OsmId, type: typeof(LongGraphType))
                .Description("The OSM ID of the Node.");

            this.Field(x => x.Tags, type: typeof(TagsType))
                .Description("Tags of the Node.");

            this.Field(x => x.Location, type: typeof(GeometryType))
                .Description("Location of the Node.");

            this.FieldAsync<ListGraphType<WayNodeType>, List<WayNode>>(
                name: nameof(Node.WayNodes),
                description: "List of related WayNode objects");
        }
    }
}