namespace GisApi.ApiServer.GraphTypes.Models
{
    using GisApi.DataAccessLayer.Models;
    using GraphQL.Types;

    public class WayNodeType : ObjectGraphType<WayNode>
    {
        public WayNodeType()
        {
            Name = "WayNodes";

            Field(x => x.WayIdx)
                .Description("An integer Index of the Node in the Way");

            Field(x => x.Role, type: typeof(StringGraphType))
                .Description("The string Role of the Node.");

            Field(x => x.Way, type: typeof(NonNullGraphType<WayType>))
                .Description("Reference to Way");

            Field(x => x.Node, type: typeof(NonNullGraphType<NodeType>))
                .Description("Reference to Node");
        }
    }
}