namespace GisApi.ApiServer.GraphTypes.Models
{
    using GisApi.ApiServer.GraphTypes.Scalars;
    using GraphQL.Types;

    public class WayInputType : InputObjectGraphType
    {
        public WayInputType()
        {
            this.Name = "WayInput";
            this.Description = "Represents input object for Way";

            this.Field<IdGraphType>("id", "ID of the Way");

            this.Field<LongGraphType>("osmId", "OpenStreetMap ID if available");

            this.Field<TagsType>("tags", "Way tags as JSON object");

            this.Field<NonNullGraphType<ListGraphType<NonNullGraphType<WayNodeInputType>>>>("wayNodes",
                "List of WayNodes related with it");
        }
    }
}