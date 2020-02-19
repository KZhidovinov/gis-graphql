namespace GisApi.ApiServer.GraphTypes.Models
{
    using GraphQL.Types;

    public class WayNodeInputType : InputObjectGraphType
    {
        public WayNodeInputType()
        {
            this.Name = "WayNodesInput";
            this.Description = "Input object to enter data about Way - Node relation";

            this.Field<IntGraphType>("wayIdx", "Index of Node in Way");

            this.Field<StringGraphType>("role", "Role of the Node in the Way");

            this.Field<WayInputType>("way", "Related Way object");

            this.Field<NodeInputType>("node", "Related Node object");
        }
    }
}