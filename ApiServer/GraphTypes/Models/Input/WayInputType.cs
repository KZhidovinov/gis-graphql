namespace GisApi.ApiServer.GraphTypes.Models
{
    using GisApi.ApiServer.GraphTypes.Scalars;
    using GraphQL.Types;

    public class WayInputType : InputObjectGraphType
    {
        public WayInputType()
        {
            Name = "WayInput";
            Description = "Represents input object for Way";

            Field<IdGraphType>("id", "ID of the Way");

            Field<LongGraphType>("osmId", "OpenStreetMap ID if available");

            Field<TagsType>("tags", "Way tags as JSON object");

            Field<NonNullGraphType<ListGraphType<NonNullGraphType<WayNodeInputType>>>>("wayNodes",
                "List of WayNodes related with it");
        }
    }
}