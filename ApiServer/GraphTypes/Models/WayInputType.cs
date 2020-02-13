using GisApi.ApiServer.GraphTypes.Scalars;
using GraphQL.Types;

namespace GisApi.ApiServer.GraphTypes.Models
{
    public class WayInputType : InputObjectGraphType
    {
        public WayInputType()
        {
            Name = "WayInput";
            Field<LongGraphType>("osmId");
            Field<TagsType>("tags");

            Field<ListGraphType<WayNodeInputType>>("wayNodes");
        }
    }
}