namespace GisApi.ApiServer.GraphTypes.Scalars.ValueNodes
{
    using GraphQL.Language.AST;

    public class TagsValue : ValueNode<TagsDictionary>
    {
        public TagsValue(TagsDictionary value)
        {
            Value = value;
        }

        protected override bool Equals(ValueNode<TagsDictionary> node)
        {
            return Value.Equals(node.Value);
        }
    }
}
