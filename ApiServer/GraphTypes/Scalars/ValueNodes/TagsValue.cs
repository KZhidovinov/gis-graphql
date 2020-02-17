namespace GisApi.ApiServer.GraphTypes.Scalars.ValueNodes
{
    using GisApi.DataAccessLayer.Models;
    using GraphQL.Language.AST;

    public class TagsValue : ValueNode<TagsDictionary>
    {
        public TagsValue(TagsDictionary value)
        {
            this.Value = value;
        }

        protected override bool Equals(ValueNode<TagsDictionary> node)
        {
            return this.Value.Equals(node?.Value);
        }
    }
}
