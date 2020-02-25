namespace GisApi.ApiServer.GraphTypes.Models
{
    using GisApi.ApiServer.GraphTypes.Scalars;
    using GisApi.DataAccessLayer.Models;
    using GisApi.DataAccessLayer.Repositories;
    using GraphQL.Types;
    using NetTopologySuite.Features;

    public class WayType : ObjectGraphType<Way>
    {
        public WayType(IWayRepository repository)
        {
            this.Name = "Way";
            this.Description = "Way object";

            this.Field(x => x.Id, type: typeof(LongGraphType))
                .Description("The ID of the Way.");

            this.Field(x => x.OsmId, type: typeof(LongGraphType))
                .Description("The OSM ID of the Way.");

            this.Field(x => x.Tags, type: typeof(TagsType))
                .Description("Tags of the Way.");

            this.Field(x => x.WayNodes, type: typeof(NonNullGraphType<ListGraphType<NonNullGraphType<WayNodeType>>>))
                .Description("List of WayNodes linked with the Way.");

            this.Field<FeatureType, Feature>("feature")
                .Resolve((ctx) => repository.GetWayFeature(ctx.Source));
        }
    }
}