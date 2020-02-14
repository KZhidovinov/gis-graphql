using GraphQL.Types;

namespace GisApi.ApiServer.GraphTypes.Models
{
    public class WayNodeInputType : InputObjectGraphType
    {
        public WayNodeInputType()
        {
            Name = "WayNodesInput";
            Description = "Input object to enter data about Way - Node relation";

            Field<IntGraphType>("wayIdx", "Index of Node in Way");

            Field<StringGraphType>("role", "Role of the Node in the Way");

            Field<WayInputType>("way", "Related Way object");

            Field<NodeInputType>("node", "Related Node object");
        }
    }
}