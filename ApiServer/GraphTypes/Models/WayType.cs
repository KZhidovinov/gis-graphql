namespace GisApi.ApiServer.GraphTypes.Models
{
    using System.Linq;
    using GisApi.ApiServer.GraphTypes.Scalars;
    using GisApi.DataAccessLayer.Models;
    using GraphQL.Types;

    public class WayType : ObjectGraphType<Way>
    {
        public WayType()
        {
            Name = "Way";
            Description = "Way object";

            Field(x => x.Id, type: typeof(IdGraphType))
                .Description("The ID of the Way.");

            Field(x => x.OsmId, type: typeof(LongGraphType))
                .Description("The OSM ID of the Way.");

            Field(x => x.Tags, type: typeof(TagsType))
                .Description("Tags of the Way.");

            Field(x => x.WayNodes, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<WayNodeType>>>))
                .Description("List of WayNode objects");

            Field(
                name: "nodes",
                description: "Nodes what Way contains",
                type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<NodeType>>>),
                resolve: context => context.Source.WayNodes
                                                    .OrderBy(wn => wn.WayIdx)
                                                    .Select(wn => wn.Node)
                );
        }
    }
}