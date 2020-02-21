namespace GisApi.ApiServer.GraphTypes.Models
{
    using System.Collections.Generic;
    using GisApi.ApiServer.GraphTypes.Scalars;
    using GisApi.DataAccessLayer.Models;
    using GraphQL.Types;

    public class WayType : ObjectGraphType<Way>
    {
        public WayType()
        {
            this.Name = "Way";
            this.Description = "Way object";

            this.Field(x => x.Id, type: typeof(LongGraphType))
                .Description("The ID of the Way.");

            this.Field(x => x.OsmId, type: typeof(LongGraphType))
                .Description("The OSM ID of the Way.");

            this.Field(x => x.Tags, type: typeof(TagsType))
                .Description("Tags of the Way.");

            this.FieldAsync<NonNullGraphType<ListGraphType<NonNullGraphType<WayNodeType>>>, List<WayNode>>(
                name: nameof(Way.WayNodes),
                description: "List of WayNodes linked with the Way.");
        }
    }
}