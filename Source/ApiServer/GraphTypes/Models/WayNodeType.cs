namespace GisApi.ApiServer.GraphTypes.Models
{
    using GisApi.DataAccessLayer.Models;
    using GraphQL.Types;

    public class WayNodeType : ObjectGraphType<WayNode>
    {
        public WayNodeType()
        {
            this.Name = "WayNodes";

            this.Field(x => x.WayIdx)
                .Description("An integer Index of the Node in the Way");

            this.Field(x => x.Role, type: typeof(StringGraphType))
                .Description("The string Role of the Node.");

            this.Field(x => x.Way, type: typeof(NonNullGraphType<WayType>))
                .Description("Reference to Way");

            this.Field(x => x.Node, type: typeof(NonNullGraphType<NodeType>))
                .Description("Reference to Node");
        }
    }
}