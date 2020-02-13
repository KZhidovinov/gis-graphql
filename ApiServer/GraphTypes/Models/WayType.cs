using GisApi.ApiServer.GraphTypes.Scalars;
using GisApi.DataAccessLayer.Models;
using GraphQL.Types;

namespace GisApi.ApiServer.GraphTypes.Models
{
    public class WayType : ObjectGraphType<Way>
    {
        public WayType()
        {
            Name = "Way";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Way.");
            Field(x => x.OsmId, type: typeof(LongGraphType)).Description("The OSM ID of the Way.");
            Field(x => x.Tags, type: typeof(TagsType)).Description("Tags of the Way.");

            Field(x => x.WayNodes, type: typeof(ListGraphType<WayNodeType>)).Description("List of WayNode objects");
        }
    }
}